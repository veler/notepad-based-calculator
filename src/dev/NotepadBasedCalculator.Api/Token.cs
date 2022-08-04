namespace NotepadBasedCalculator.Api
{
    /// <summary>
    /// Represents a token.
    /// </summary>
    public sealed class Token
    {
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

        public Token(TokenType type, int startIndex, int endIndex)
        {
            Guard.IsGreaterThanOrEqualTo(startIndex, 0);
            Guard.IsGreaterThan(endIndex, startIndex);

            Type = type;
            StartIndex = startIndex;
            EndIndex = endIndex;
            Length = endIndex - startIndex;
        }

        public override string ToString()
        {
            return $"{Type} ({StartIndex}, {EndIndex})";
        }
    }
}
