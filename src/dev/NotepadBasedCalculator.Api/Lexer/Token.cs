namespace NotepadBasedCalculator.Api
{
    /// <summary>
    /// Represents a token.
    /// </summary>
    public record Token : IToken
    {
        private readonly string _lineTextIncludingLineBreak;

        public string Type { get; }

        public int StartInLine { get; }

        public int EndInLine { get; }

        public int Length => EndInLine - StartInLine;

        internal Token(string lineTextIncludingLineBreak, int startInLine, int endInLine, string tokenType)
        {
            Guard.IsGreaterThanOrEqualTo(startInLine, 0);
            Guard.IsGreaterThan(endInLine, startInLine);
            Guard.IsNotNullOrEmpty(lineTextIncludingLineBreak);
            Guard.IsNotNullOrWhiteSpace(tokenType);
            _lineTextIncludingLineBreak = lineTextIncludingLineBreak;

            Type = tokenType;
            StartInLine = startInLine;
            EndInLine = endInLine;
        }

        public bool IsTokenTextEqualTo(string compareTo, StringComparison comparisonType)
        {
            if (string.IsNullOrEmpty(compareTo) || compareTo.Length != Length)
                return false;

            return _lineTextIncludingLineBreak.IndexOf(compareTo, StartInLine, Length, comparisonType) == StartInLine;
        }

        public string GetText()
        {
            return _lineTextIncludingLineBreak.Substring(StartInLine, Length);
        }

        public override string ToString()
        {
            return $"[{Type}] ({StartInLine}, {EndInLine}): \"{GetText()}\"";
        }
    }
}
