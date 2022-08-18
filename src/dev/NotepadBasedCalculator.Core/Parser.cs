using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.Recognizers.Text;
using NotepadBasedCalculator.Api;

namespace NotepadBasedCalculator.Core
{
    [Export]
    internal sealed class Parser
    {
        private readonly ILogger _logger;
        private readonly IParserRepository _parserRepository;
        private readonly Lexer _lexer;

        [ImportingConstructor]
        public Parser(ILogger logger, IParserRepository parserRepository, Lexer lexer)
        {
            _logger = logger;
            _parserRepository = parserRepository;
            _lexer = lexer;
        }

        internal Task<ParserResult?> ParseAsync(string? input, CancellationToken cancellationToken = default)
        {
            return ParseAsync(input, SupportedCultures.English, cancellationToken);
        }

        internal async Task<ParserResult?> ParseAsync(string? input, string culture, CancellationToken cancellationToken)
        {
            Guard.IsNotNullOrWhiteSpace(culture);
            culture = Culture.MapToNearestLanguage(culture);

            var resultLines = new List<ParserResultLine>();
            IReadOnlyList<TokenizedTextLine> tokenizedLines = _lexer.Tokenize(culture, input);

            for (int i = 0; i < tokenizedLines.Count; i++)
            {
                TokenizedTextLine tokenizedLine = tokenizedLines[i];

                ParserResultLine parserResultLine
                    = await ParseLineAsync(
                        culture,
                        tokenizedLine,
                        AggregateAllKnownVariableNames(resultLines),
                        cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                resultLines.Add(parserResultLine);
            }

            return new ParserResult(resultLines);
        }

        internal async Task<ParserResult?> ParseAndMergeWithOlderResultAsync(
            ParserResult? oldParserResult,
            IReadOnlyList<TokenizedTextLine> newTokenizedTextLines,
            int lineFromWhichSomethingHasChanged,
            string culture,
            CancellationToken cancellationToken)
        {
            Guard.IsInRange(lineFromWhichSomethingHasChanged, 0, newTokenizedTextLines.Count);
            Guard.IsNotNull(newTokenizedTextLines);
            Guard.IsNotNullOrWhiteSpace(culture);
            culture = Culture.MapToNearestLanguage(culture);

            var resultLines = new List<ParserResultLine>();

            if (oldParserResult is not null)
            {
                for (int i = 0; i < lineFromWhichSomethingHasChanged; i++)
                {
                    resultLines.Add(oldParserResult.Lines[i]);
                }
            }

            for (int i = lineFromWhichSomethingHasChanged; i < newTokenizedTextLines.Count; i++)
            {
                TokenizedTextLine tokenizedLine = newTokenizedTextLines[i];

                ParserResultLine parserResultLine
                    = await ParseLineAsync(
                        culture,
                        tokenizedLine,
                        AggregateAllKnownVariableNames(resultLines),
                        cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                resultLines.Add(parserResultLine);
            }

            return new ParserResult(resultLines);
        }

        private async Task<ParserResultLine> ParseLineAsync(string culture, TokenizedTextLine tokenizedLine, IReadOnlyList<string> orderedKnownVariableNames, CancellationToken cancellationToken)
        {
            IReadOnlyList<IData> parsedData = await ParseDataAsync(culture, tokenizedLine, cancellationToken);

            tokenizedLine = _lexer.TokenizeLine(culture, tokenizedLine.Start, tokenizedLine.LineTextIncludingLineBreak, orderedKnownVariableNames, parsedData);

            IReadOnlyList<Statement> statements = ParseStatements(culture, tokenizedLine, cancellationToken);

            return new ParserResultLine(tokenizedLine, parsedData, statements);
        }

        private IReadOnlyList<Statement> ParseStatements(string culture, TokenizedTextLine tokenizedLine, CancellationToken cancellationToken)
        {
            var statements = new List<Statement>();

            LinkedToken? nextTokenToParse = tokenizedLine.Tokens;
            while (nextTokenToParse is not null && !cancellationToken.IsCancellationRequested)
            {
                Statement? statement = ParseNextStatement(culture, nextTokenToParse, cancellationToken);
                if (statement is not null)
                {
                    nextTokenToParse = statement.LastToken.Next;
                    statements.Add(statement);
                }
                else
                {
                    // Ignore the current token. It might be a word that we would simply skip.
                    nextTokenToParse = nextTokenToParse.Next;
                }
            }

            return statements;
        }

        private Statement? ParseNextStatement(string culture, LinkedToken linkedToken, CancellationToken cancellationToken)
        {
            Statement? statement = null;

            foreach (IStatementParser statementParser in _parserRepository.GetApplicableStatementParsers(culture))
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                try
                {
                    if (statementParser.TryParseStatement(culture, linkedToken, out statement)
                        && statement is not null)
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogFault(
                        "Parser.ParseNextStatement.Fault",
                        ex,
                        ("ParserName", statementParser.GetType().FullName));
                }
            }

            return statement;
        }

        private async Task<IReadOnlyList<IData>> ParseDataAsync(string culture, TokenizedTextLine tokenizedLine, CancellationToken cancellationToken)
        {
            var rawDataBag = new List<IData>();
            var nonOverlappingData = new List<IData>();
            var tasks = new List<Task>();

            foreach (IDataParser dataParser in _parserRepository.GetApplicableDataParsers(culture))
            {
                tasks.Add(
                    Task.Run(
                        () =>
                        {
                            try
                            {
                                IReadOnlyList<IData>? results = dataParser.Parse(culture, tokenizedLine);
                                if (results is not null)
                                {
                                    lock (rawDataBag)
                                    {
                                        rawDataBag.AddRange(results);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogFault(
                                    "Parser.ParseData.Fault",
                                    ex,
                                    ("DataParserName", dataParser.GetType().FullName));
                            }
                        }));
            }

            if (cancellationToken.IsCancellationRequested)
            {
                return nonOverlappingData;
            }

            await Task.WhenAll(tasks);

            if (cancellationToken.IsCancellationRequested)
            {
                return nonOverlappingData;
            }

            // For each data we parsed, find whether the data is overlapped by another one. If not, then we keep it.
            for (int i = 0; i < rawDataBag.Count; i++)
            {
                IData currentData = rawDataBag[i];
                if (currentData is not null
                    && !IsDataOverlapped(rawDataBag, currentData))
                {
                    nonOverlappingData.Add(currentData);
                }
            }

            // Sort the non-overlapping items.
            nonOverlappingData.Sort();

            return nonOverlappingData;
        }

        private static bool IsDataOverlapped(IReadOnlyList<IData> allData, IData currentData)
        {
            for (int i = 0; i < allData.Count; i++)
            {
                IData data = allData[i];
                if (data is not null
                    && currentData != data
                    && data.StartInLine <= currentData.StartInLine
                    && data.EndInLine >= currentData.EndInLine)
                {
                    return true;
                }
            }

            return false;
        }

        private static IReadOnlyList<string> AggregateAllKnownVariableNames(IReadOnlyList<ParserResultLine> parsedLines)
        {
            var knownVariableNames = new HashSet<string>();
            for (int i = 0; i < parsedLines.Count; i++)
            {
                ParserResultLine parsedLine = parsedLines[i];
                for (int j = 0; j < parsedLine.Statements.Count; j++)
                {
                    if (parsedLine.Statements[j] is VariableDeclarationStatement variableDeclarationStatement)
                    {
                        knownVariableNames.Add(variableDeclarationStatement.VariableName);
                    }
                }
            }

            return knownVariableNames.ToImmutableSortedSet(new DescendingComparer<string>());
        }
    }
}
