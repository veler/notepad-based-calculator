using Microsoft.Recognizers.Text;

namespace NotepadBasedCalculator.Core
{
    internal sealed class Interpreter
    {
        private readonly string _culture;
        private readonly Lexer _lexer;
        private readonly Parser _parser;
        private readonly TextDocument _textDocument;

        private CancellationTokenSource _cancellationTokenSource = new();
        private ParserResult? _parserResult = null;

        internal Interpreter(string culture, Lexer lexer, Parser parser, TextDocument textDocument)
        {
            Guard.IsNotNullOrWhiteSpace(culture);
            Guard.IsNotNull(lexer);
            Guard.IsNotNull(parser);
            Guard.IsNotNull(textDocument);
            _culture = Culture.MapToNearestLanguage(culture);
            _lexer = lexer;
            _parser = parser;
            _textDocument = textDocument;
            _textDocument.TextChanged += TextDocument_TextChanged;
        }

        private void TextDocument_TextChanged(object sender, EventArgs e)
        {
            CancelCurrentInterpretationWork();
            CancellationToken cancellationToken = _cancellationTokenSource.Token;

            string newText = _textDocument.Text;
            IntepretAsync(newText, _parserResult, cancellationToken).ForgetSafely();
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

            IReadOnlyList<TokenizedTextLine> newTokenizedTextLines = _lexer.Tokenize(_culture, text);

            int lineFromWhichSomethingHasChanged = DetermineLineFromWhichSomethingHasChanged(oldParserResult, newTokenizedTextLines);

            ParserResult? parserResult
                = await _parser.ParseAndMergeWithOlderResultAsync(
                    oldParserResult,
                    newTokenizedTextLines,
                    lineFromWhichSomethingHasChanged,
                    _culture,
                    cancellationToken);

            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            Guard.IsNotNull(parserResult);

            Interlocked.Exchange(ref _parserResult, parserResult);
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

            return Math.Min(oldParserResult?.Lines.Count ?? 0, newTokenizedTextLines.Count);
        }
    }
}
