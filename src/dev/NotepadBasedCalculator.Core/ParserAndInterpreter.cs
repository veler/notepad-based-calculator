using Microsoft.Recognizers.Text;

namespace NotepadBasedCalculator.Core
{
    internal sealed class ParserAndInterpreter : IDisposable
    {
        private readonly string _culture;
        private readonly ILogger _logger;
        private readonly Lexer _lexer;
        private readonly IParserRepository _parserRepository;
        private readonly VariableService _variableService = new();
        private readonly TextDocument _textDocument;

        private CancellationTokenSource _cancellationTokenSource = new();
        private Task _currentParsingAndInterpretationTask = Task.CompletedTask;
        private IReadOnlyList<ParserAndInterpreterResultLine>? _lineResults = null;
        private IReadOnlyList<TokenizedTextLine>? _oldTokenizedTextLines;

        internal ParserAndInterpreter(
            string culture,
            ILogger logger,
            ILexer lexer,
            IParserRepository parserRepository,
            TextDocument textDocument)
        {
            Guard.IsNotNullOrWhiteSpace(culture);
            Guard.IsNotNull(logger);
            Guard.IsNotNull(lexer);
            Guard.IsNotNull(parserRepository);
            Guard.IsNotNull(textDocument);

            _culture = Culture.MapToNearestLanguage(culture);

            _logger = logger;
            _lexer = (Lexer)lexer;
            _parserRepository = parserRepository;
            _textDocument = textDocument;
            _textDocument.TextChanged += TextDocument_TextChanged;
        }

        public void Dispose()
        {
            _textDocument.TextChanged -= TextDocument_TextChanged;
            CancelCurrentInterpretationWork();
        }

        internal async Task<IReadOnlyList<ParserAndInterpreterResultLine>?> WaitAsync()
        {
            try
            {
                await _currentParsingAndInterpretationTask.ConfigureAwait(true);
            }
            catch (OperationCanceledException)
            {
                // Ignore.
            }

            return _lineResults;
        }

        private void TextDocument_TextChanged(object? sender, EventArgs e)
        {
            CancelCurrentInterpretationWork();
            CancellationToken cancellationToken = _cancellationTokenSource.Token;

            string newText = _textDocument.Text;
            _currentParsingAndInterpretationTask = ParseAndIntepretAsync(newText, cancellationToken);
        }

        private void CancelCurrentInterpretationWork()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = new();
        }

        private async Task ParseAndIntepretAsync(string text, CancellationToken cancellationToken)
        {
            // Make sure to go off the UI thread.
            await TaskScheduler.Default;
            cancellationToken.ThrowIfCancellationRequested();

            // Tokenize the whole document.
            IReadOnlyList<TokenizedTextLine> newTokenizedTextLines = _lexer.Tokenize(_culture, text);

            // Determine on which line a change happened.
            int lineFromWhichSomethingHasChanged = DetermineLineFromWhichSomethingHasChanged(_oldTokenizedTextLines, newTokenizedTextLines);

            // For each line to parse and interpret.
            var lineResults = new List<ParserAndInterpreterResultLine>();
            for (int i = lineFromWhichSomethingHasChanged; i < newTokenizedTextLines.Count; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                ParserAndInterpreterResultLine lineResult = await ParseAndInterpretLineAsync(newTokenizedTextLines[i], cancellationToken);
                lineResults.Add(lineResult);
            }

            Interlocked.Exchange(ref _oldTokenizedTextLines, newTokenizedTextLines);
            Interlocked.Exchange(ref _lineResults, lineResults);
        }

        private async Task<ParserAndInterpreterResultLine> ParseAndInterpretLineAsync(TokenizedTextLine lineToParseAndInterpret, CancellationToken cancellationToken)
        {
            // Re-tokenize the line by taking consideration of the data this time.
            lineToParseAndInterpret = await TokenizeTextLineWithDataDetectionAsync(lineToParseAndInterpret, cancellationToken);
            if (lineToParseAndInterpret.Tokens is null)
            {
                return new ParserAndInterpreterResultLine(lineToParseAndInterpret);
            }

            // Start recording all the variable changes while interpreting the line. The changes won't be saved if the operation gets canceled.
            using (_variableService.BeginRecordVariableSnapshot(lineToParseAndInterpret.LineNumber, cancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Try to parse and interpret the line.
                var statementResults = new List<StatementParserAndInterpreterResult>();
                LinkedToken? nextTokenToParse = lineToParseAndInterpret.Tokens;
                while (nextTokenToParse is not null && !cancellationToken.IsCancellationRequested)
                {
                    // Parse and interpret the next statement in the line.
                    StatementParserAndInterpreterResult? result = await ParseAndInterpretNextStatementAsync(nextTokenToParse, cancellationToken);
                    if (result is not null)
                    {
                        Guard.IsNotNull(result.ParsedStatement);
                        nextTokenToParse = result.ParsedStatement.LastToken.Next;
                        statementResults.Add(result);
                    }
                    else
                    {
                        // Ignore the current token. It might be a word that we would simply skip.
                        nextTokenToParse = nextTokenToParse.Next;
                    }
                }

                cancellationToken.ThrowIfCancellationRequested();

                // If there were multiple statements on the line, let's do the addition of each data produced.
                if (statementResults.Count > 1)
                {
                    IData? mergedResult = null;
                    for (int i = 0; i < statementResults.Count; i++)
                    {
                        // TODO
                    }

                    return new ParserAndInterpreterResultLine(lineToParseAndInterpret, statementResults, mergedResult);
                }
                else if (statementResults.Count == 1)
                {
                    return new ParserAndInterpreterResultLine(lineToParseAndInterpret, statementResults, statementResults[0].ResultedData);
                }

                return new ParserAndInterpreterResultLine(lineToParseAndInterpret, statementResults);
            }
        }

        private async Task<StatementParserAndInterpreterResult?> ParseAndInterpretNextStatementAsync(LinkedToken currentToken, CancellationToken cancellationToken)
        {
            // Get applicable statement parsers and interpreters.
            IEnumerable<IStatementParserAndInterpreter> statementParsersAndInterpreters
                = _parserRepository.GetApplicableStatementParsersAndInterpreters(_culture);

            foreach (IStatementParserAndInterpreter statementParserAndInterpreter in statementParsersAndInterpreters)
            {
                try
                {
                    // Try to parse and interpret a statement.
                    var result = new StatementParserAndInterpreterResult();
                    if (await statementParserAndInterpreter.TryParseAndInterpretStatementAsync(_culture, currentToken, _variableService, result, cancellationToken))
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        if (result.ParsedStatement is null)
                        {
                            ThrowHelper.ThrowInvalidDataException(
                                $"The method '{nameof(statementParserAndInterpreter.TryParseAndInterpretStatementAsync)}' returned true " +
                                $"but '{nameof(StatementParserAndInterpreterResult)}.{nameof(StatementParserAndInterpreterResult.ParsedStatement)}' is null. " +
                                $"It should not be null.");
                        }

                        return result;
                    }
                }
                catch (OperationCanceledException)
                {
                    // Ignore.
                }
                catch (Exception ex)
                {
                    _logger.LogFault(
                        "ParseAndInterpretStatement.Fault",
                        ex,
                        ("StatementParserAndInterpreterName", statementParserAndInterpreter.GetType().FullName));
                }
            }

            return null;
        }

        private async Task<TokenizedTextLine> TokenizeTextLineWithDataDetectionAsync(TokenizedTextLine tokenizedLine, CancellationToken cancellationToken)
        {
            IReadOnlyList<IData> parsedData
                = await ParseDataAsync(
                    tokenizedLine,
                    cancellationToken)
                .ConfigureAwait(true);

            return _lexer.TokenizeLine(
                _culture,
                tokenizedLine.LineNumber,
                tokenizedLine.Start,
                tokenizedLine.LineTextIncludingLineBreak,
                _variableService.GetVariableNames(),
                parsedData);
        }

        private async Task<IReadOnlyList<IData>> ParseDataAsync(TokenizedTextLine tokenizedLine, CancellationToken cancellationToken)
        {
            var rawDataBag = new List<IData>();
            var nonOverlappingData = new List<IData>();
            var tasks = new List<Task>();

            foreach (IDataParser dataParser in _parserRepository.GetApplicableDataParsers(_culture))
            {
                tasks.Add(
                    Task.Run(
                        () =>
                        {
                            try
                            {
                                IReadOnlyList<IData>? results = dataParser.Parse(_culture, tokenizedLine, cancellationToken);
                                if (results is not null)
                                {
                                    lock (rawDataBag)
                                    {
                                        rawDataBag.AddRange(results);
                                    }
                                }
                            }
                            catch (OperationCanceledException)
                            {
                                // Ignore.
                            }
                            catch (Exception ex)
                            {
                                _logger.LogFault(
                                    "Parser.ParseData.Fault",
                                    ex,
                                    ("DataParserName", dataParser.GetType().FullName));
                            }
                        },
                        cancellationToken));
            }

            await Task.WhenAny(Task.WhenAll(tasks), cancellationToken.AsTask()).ConfigureAwait(true);

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

        private static int DetermineLineFromWhichSomethingHasChanged(
            IReadOnlyList<TokenizedTextLine>? oldTokenizedTextLines,
            IReadOnlyList<TokenizedTextLine> newTokenizedTextLines)
        {
            Guard.IsNotNull(newTokenizedTextLines);

            if (oldTokenizedTextLines is not null)
            {
                int i = 0;
                while (i < oldTokenizedTextLines.Count && i < newTokenizedTextLines.Count)
                {
                    TokenizedTextLine oldTokenizedTextLine = oldTokenizedTextLines[i];
                    TokenizedTextLine tokenizedTextLine = newTokenizedTextLines[i];

                    if (!string.Equals(
                        oldTokenizedTextLine.LineTextIncludingLineBreak,
                        tokenizedTextLine.LineTextIncludingLineBreak,
                        StringComparison.Ordinal))
                    {
                        return i;
                    }

                    i++;
                }
            }

            return Math.Max(Math.Min(oldTokenizedTextLines?.Count ?? 0, newTokenizedTextLines.Count) - 1, 0);
        }
    }
}
