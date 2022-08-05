namespace NotepadBasedCalculator.Api
{
    /// <summary>
    /// Represents a token.
    /// </summary>
    public sealed class Token
    {
        private string _wholeText;

        /// <summary>
        /// Gets the type of token.
        /// </summary>
        public TokenType Type { get; }

        /// <summary>
        /// Gets the position in the document where the token starts.
        /// </summary>
        public int StartIndex { get; }

        /// <summary>
        /// Gets the position in the document where the token ends.
        /// </summary>
        public int EndIndex { get; }

        /// <summary>
        /// Gets the length of the token.
        /// </summary>
        public int Length { get; }

        internal Token(string input, TokenType type, int startIndex, int endIndex)
        {
            Guard.IsGreaterThanOrEqualTo(startIndex, 0);
            Guard.IsGreaterThan(endIndex, startIndex);
            Guard.IsNotNullOrEmpty(input);
            _wholeText = input;

            Type = type;
            StartIndex = startIndex;
            EndIndex = endIndex;
            Length = endIndex - startIndex;
        }

        public override string ToString()
        {
            return $"{Type} ({StartIndex}, {EndIndex})";
        }

        public string GetText()
        {
            return _wholeText.Substring(StartIndex, Length);
        }

        public bool IsTokenTextEqualTo(string compareTo, StringComparison comparisonType)
        {
            if (string.IsNullOrEmpty(compareTo) || compareTo.Length != Length)
            {
                return false;
            }

            return _wholeText.IndexOf(compareTo, StartIndex, Length, comparisonType) == StartIndex;
        }
    }
}
