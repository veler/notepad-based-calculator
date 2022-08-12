namespace NotepadBasedCalculator.Api
{
    public interface IToken
    {
        /// <summary>
        /// Gets an internal non-localized name that represents the type of token.
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// Gets the position in the line where the token starts.
        /// </summary>
        public int StartInLine { get; }

        /// <summary>
        /// Gets the position in the line where the token ends.
        /// </summary>
        public int EndInLine { get; }

        /// <summary>
        /// Gets the length of the token.
        /// </summary>
        public int Length { get; }

        bool IsTokenTextEqualTo(string compareTo, StringComparison comparisonType);

        string GetText();
    }
}
