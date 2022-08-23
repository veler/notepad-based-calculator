using Microsoft.Recognizers.Text;

namespace NotepadBasedCalculator.Core
{
    internal sealed class Interpreter : IExpressionInterpreter, IDisposable
    {
        private readonly string _culture;
        private readonly ILexer _lexer;
        private readonly Parser _parser;
        private readonly IEnumerable<Lazy<IStatementInterpreter, InterpreterMetadata>> _statementInterpreters;
        private readonly IEnumerable<Lazy<IExpressionInterpreter, InterpreterMetadata>> _expressionInterpreters;
        private readonly VariableService _variableService = new VariableService();
        private readonly List<IReadOnlyDictionary<string, IData?>> _variablePerLineBackup = new();
        private readonly TextDocument _textDocument;

        private CancellationTokenSource _cancellationTokenSource = new();
        private Task _currentInterpretationTask = Task.CompletedTask;
        private ParserResult? _parserResult = null;
        private IReadOnlyList<IData?>? _lineResults = null;

        internal Interpreter(
            string culture,
            ILexer lexer,
            Parser parser,
            IEnumerable<Lazy<IStatementInterpreter, InterpreterMetadata>> statementInterpreters,
            IEnumerable<Lazy<IExpressionInterpreter, InterpreterMetadata>> expressionInterpreters,
            TextDocument textDocument)
        {
            Guard.IsNotNullOrWhiteSpace(culture);
            Guard.IsNotNull(lexer);
            Guard.IsNotNull(parser);
            Guard.IsNotNull(statementInterpreters);
            Guard.IsNotNull(expressionInterpreters);
            Guard.IsNotNull(textDocument);
            _culture = Culture.MapToNearestLanguage(culture);
            _lexer = lexer;
            _parser = parser;
            _statementInterpreters = statementInterpreters;
            _expressionInterpreters = expressionInterpreters;
            _textDocument = textDocument;
            _textDocument.TextChanged += TextDocument_TextChanged;
        }

        public void Dispose()
        {
            _textDocument.TextChanged -= TextDocument_TextChanged;
            CancelCurrentInterpretationWork();
        }

        internal async Task<IReadOnlyList<IData?>?> WaitAsync()
        {
            await _currentInterpretationTask.ConfigureAwait(true);
            return _lineResults;
        }

        private void TextDocument_TextChanged(object sender, EventArgs e)
        {
            CancelCurrentInterpretationWork();
            CancellationToken cancellationToken = _cancellationTokenSource.Token;

            string newText = _textDocument.Text;
            _currentInterpretationTask = IntepretAsync(newText, _parserResult, cancellationToken);
        }

        private void CancelCurrentInterpretationWork()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = new();
        }

        private async Task IntepretAsync(string text, ParserResult? oldParserResult, CancellationToken cancellationToken)
        {
            await TaskScheduler.Default;

            // Tokenize the whole document.
            IReadOnlyList<TokenizedTextLine> newTokenizedTextLines = _lexer.Tokenize(_culture, text);

            // Determine on which line a change happened.
            int lineFromWhichSomethingHasChanged = DetermineLineFromWhichSomethingHasChanged(oldParserResult, newTokenizedTextLines);

            // Parse the line that changes along with the one below it.
            ParserResult? parserResult
                = await _parser.ParseAndMergeWithOlderResultAsync(
                    oldParserResult,
                    newTokenizedTextLines,
                    lineFromWhichSomethingHasChanged,
                    _culture,
                    cancellationToken)
                .ConfigureAwait(true);

            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            Guard.IsNotNull(parserResult);

            if (lineFromWhichSomethingHasChanged - 1 >= 0 && lineFromWhichSomethingHasChanged - 1 < _variablePerLineBackup.Count)
            {
                _variableService.RestoreBackup(_variablePerLineBackup[lineFromWhichSomethingHasChanged - 1]);
            }
            else
            {
                _variableService.RestoreBackup(null);
            }

            // Interpret the whole document starting from the line that changed.
            var lineResults = new List<IData?>();
            for (int i = lineFromWhichSomethingHasChanged; i < parserResult.Lines.Count; i++)
            {
                IData? lineResult = await InterpretLineAsync(parserResult.Lines[i], cancellationToken).ConfigureAwait(true);
                if (_variablePerLineBackup.Count > i)
                {
                    _variablePerLineBackup[i] = _variableService.CreateBackup();
                }
                else
                {
                    _variablePerLineBackup.Add(_variableService.CreateBackup());
                }

                lineResults.Add(lineResult);

                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }
            }

            Interlocked.Exchange(ref _parserResult, parserResult);
            Interlocked.Exchange(ref _lineResults, lineResults);
        }

        private async Task<IData?> InterpretLineAsync(ParserResultLine line, CancellationToken cancellationToken)
        {
            var allData = new List<IData>();

            // Interpret each statement.
            for (int i = 0; i < line.Statements.Count; i++)
            {
                Statement statement = line.Statements[i];
                Type statementType = statement.GetType();
                IStatementInterpreter statementInterpreter = GetApplicableStatementInterpreter(statementType);

                IData? statementResult
                    = await statementInterpreter.InterpretStatementAsync(
                        _culture,
                        _variableService,
                        this,
                        statement,
                        cancellationToken)
                    .ConfigureAwait(true);

                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                if (statementResult is not null)
                {
                    allData.Add(statementResult);
                }
            }

            // If there were multiple statements on the line, let's do the addition of each data produced.
            if (allData.Count > 1)
            {
                for (int i = 0; i < allData.Count; i++)
                {
                    // TODO
                }
                return null;
            }
            else if (allData.Count == 1)
            {
                return allData[0];
            }
            else
            {
                return null;
            }
        }

        async Task<IData?> IExpressionInterpreter.InterpretExpressionAsync(
            string culture,
            IVariableService variableService,
            IExpressionInterpreter expressionInterpreter,
            Expression expression,
            CancellationToken cancellationToken)
        {
            Guard.IsNotNullOrWhiteSpace(culture);
            Guard.IsNotNull(variableService);

            if (expression is null || cancellationToken.IsCancellationRequested)
            {
                return null;
            }

            Type expressionType = expression.GetType();
            IExpressionInterpreter interpreter = GetApplicableExpressionInterpreter(expressionType);

            if (interpreter is null)
            {
                return null;
            }

            IData? expressionResult
                = await interpreter.InterpretExpressionAsync(
                    culture,
                    variableService,
                    this,
                    expression,
                    cancellationToken)
                .ConfigureAwait(true);

            return expressionResult;
        }

        private static int DetermineLineFromWhichSomethingHasChanged(ParserResult? oldParserResult, IReadOnlyList<TokenizedTextLine> newTokenizedTextLines)
        {
            Guard.IsNotNull(newTokenizedTextLines);

            if (oldParserResult is not null)
            {
                int i = 0;
                while (i < oldParserResult.Lines.Count && i < newTokenizedTextLines.Count)
                {
                    TokenizedTextLine oldTokenizedTextLine = oldParserResult.Lines[i].TokenizedTextLine;
                    TokenizedTextLine tokenizedTextLine = newTokenizedTextLines[i];

                    if (!string.Equals(oldTokenizedTextLine.LineTextIncludingLineBreak, tokenizedTextLine.LineTextIncludingLineBreak, StringComparison.Ordinal))
                    {
                        return i;
                    }

                    i++;
                }
            }

            return Math.Max(Math.Min(oldParserResult?.Lines.Count ?? 0, newTokenizedTextLines.Count) - 1, 0);
        }

        private IStatementInterpreter GetApplicableStatementInterpreter(Type type)
        {
            return _statementInterpreters.Where(
                    p => p.Metadata.CultureCodes.Any(c => CultureHelper.IsCultureApplicable(c, _culture))
                        && p.Metadata.Types.Any(t => t == type))
                    .Single().Value;
        }

        private IExpressionInterpreter GetApplicableExpressionInterpreter(Type type)
        {
            return _expressionInterpreters.Where(
                    p => p.Metadata.CultureCodes.Any(c => CultureHelper.IsCultureApplicable(c, _culture))
                        && p.Metadata.Types.Any(t => t == type))
                    .Single().Value;
        }
    }
}
