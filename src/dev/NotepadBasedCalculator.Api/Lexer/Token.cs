namespace NotepadBasedCalculator.Api
{
    /// <summary>
    /// Represents a token.
    /// </summary>
    public sealed record Token
    {
        private readonly string _lineTextIncludingLineBreak;

        /// <summary>
        /// Gets the type of token.
        /// </summary>
        public TokenType Type { get; }

        /// <summary>
        /// Gets the position in the document where the token starts.
        /// </summary>
        public int StartInLine { get; }

        /// <summary>
        /// Gets the position in the document where the token ends.
        /// </summary>
        public int EndInLine { get; }

        /// <summary>
        /// Gets the length of the token.
        /// </summary>
        public int Length { get; }

        internal Token(string lineTextIncludingLineBreak, TokenType type, int startInLine, int endInLine)
        {
            Guard.IsGreaterThanOrEqualTo(startInLine, 0);
            Guard.IsGreaterThan(endInLine, startInLine);
            Guard.IsNotNullOrEmpty(lineTextIncludingLineBreak);
            _lineTextIncludingLineBreak = lineTextIncludingLineBreak;

            Type = type;
            StartInLine = startInLine;
            EndInLine = endInLine;
            Length = endInLine - startInLine;
        }

        public override string ToString()
        {
            return $"{Type} ({StartInLine}, {EndInLine}): \"{GetText()}\"";
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
    }
}
