using Microsoft.Recognizers.Text;

namespace NotepadBasedCalculator.Core
{
    [Export]
    internal sealed class Parser
    {
        private readonly IParserRepository _parserRepository;

        [ImportingConstructor]
        public Parser(IParserRepository parserRepository)
        {
            _parserRepository = parserRepository;
        }

        internal Task<ParserResult> ParseAsync(string? input)
        {
            return ParseAsync(input, SupportedCultures.English);
        }

        internal async Task<ParserResult> ParseAsync(string? input, string culture)
        {
            Guard.IsNotNullOrWhiteSpace(culture);
            culture = Culture.MapToNearestLanguage(culture);

            var resultLines = new List<ParserResultLine>();
            IReadOnlyList<TokenizedTextLine> tokenizedLines = Lexer.Tokenize(input);

            for (int i = 0; i < tokenizedLines.Count; i++)
            {
                TokenizedTextLine tokenizedLine = tokenizedLines[i];

                ParserResultLine parserResultLine = await ParseLineAsync(culture, tokenizedLine);

                resultLines.Add(parserResultLine);
            }

            return new ParserResult(resultLines);
        }

        private async Task<ParserResultLine> ParseLineAsync(string culture, TokenizedTextLine tokenizedLine)
        {
            IReadOnlyList<IData> parsedData = await ParseDataAsync(culture, tokenizedLine);

            tokenizedLine = Lexer.TokenizeLine(tokenizedLine.Start, tokenizedLine.LineTextIncludingLineBreak, parsedData);

            IReadOnlyList<Statement> statements = ParseStatements(culture, tokenizedLine);

            return new ParserResultLine(tokenizedLine, parsedData, statements);
        }

        private IReadOnlyList<Statement> ParseStatements(string culture, TokenizedTextLine tokenizedLine)
        {
            var statements = new List<Statement>();

            LinkedToken? nextTokenToParse = tokenizedLine.Tokens;
            while (nextTokenToParse is not null)
            {
                Statement? statement = ParseNextStatement(culture, nextTokenToParse);
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

        private Statement? ParseNextStatement(string culture, LinkedToken linkedToken)
        {
            Statement? statement = null;

            foreach (IStatementParser statementParser in _parserRepository.GetApplicableStatementParsers(culture))
            {
                if (statementParser.TryParseStatement(culture, linkedToken, out statement)
                    && statement is not null)
                {
                    break;
                }
            }

            return statement;
        }

        private async Task<IReadOnlyList<IData>> ParseDataAsync(string culture, TokenizedTextLine tokenizedLine)
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
                            IReadOnlyList<IData>? results = dataParser.Parse(culture, tokenizedLine);
                            if (results is not null)
                            {
                                lock (rawDataBag)
                                {
                                    rawDataBag.AddRange(results);
                                }
                            }
                        }));
            }

            await Task.WhenAll(tasks);

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
    }
}
