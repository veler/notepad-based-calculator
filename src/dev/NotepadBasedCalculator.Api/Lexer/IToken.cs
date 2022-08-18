namespace NotepadBasedCalculator.Api
{
    public interface IToken
    {
        /// <summary>
        /// Gets an internal non-localized, sensitive name that represents the type of token.
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

        bool IsNot(string type);

        bool Is(string expectedType);

        bool Is(string expectedType, string expectedTokenText, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase);

        bool Is(string expectedType, string[] expectedTokenText, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase);

        bool IsTokenTextEqualTo(string compareTo, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase);

        string GetText();

        string GetText(int startInLine, int endInLine);
    }
}
