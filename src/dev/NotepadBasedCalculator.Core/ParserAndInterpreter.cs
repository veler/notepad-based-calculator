using Microsoft.Recognizers.Text;

namespace NotepadBasedCalculator.Core
{
    internal sealed class ParserAndInterpreter : IDisposable
    {
        private readonly string _culture;
        private readonly ILexer _lexer;
        private readonly VariableService _variableService = new VariableService();
        private readonly List<IReadOnlyDictionary<string, IData?>> _variablePerLineBackup = new();
        private readonly TextDocument _textDocument;

        private CancellationTokenSource _cancellationTokenSource = new();
        private Task _currentParsingAndInterpretationTask = Task.CompletedTask;
        private IReadOnlyList<IData?>? _lineResults = null;
        private IReadOnlyList<TokenizedTextLine>? _oldTokenizedTextLines;

        internal ParserAndInterpreter(
            string culture,
            ILexer lexer,
            TextDocument textDocument)
        {
            Guard.IsNotNullOrWhiteSpace(culture);
            Guard.IsNotNull(lexer);
            Guard.IsNotNull(textDocument);
            _culture = Culture.MapToNearestLanguage(culture);
            _lexer = lexer;
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
            await _currentParsingAndInterpretationTask.ConfigureAwait(true);
            return _lineResults;
        }

        private void TextDocument_TextChanged(object sender, EventArgs e)
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

            // Tokenize the whole document.
            IReadOnlyList<TokenizedTextLine> newTokenizedTextLines = _lexer.Tokenize(_culture, text);

            // Determine on which line a change happened.
            int lineFromWhichSomethingHasChanged = DetermineLineFromWhichSomethingHasChanged(_oldTokenizedTextLines, newTokenizedTextLines);
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
