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

        public bool IsNot(string type)
        {
            return !Is(type);
        }

        public bool Is(string expectedType)
        {
            return string.Equals(Type, expectedType, StringComparison.Ordinal);
        }

        public bool Is(string expectedType, string expectedTokenText, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
        {
            return Is(expectedType) && IsTokenTextEqualTo(expectedTokenText, comparisonType);
        }

        public bool Is(string expectedType, string[] expectedTokenTexts, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
        {
            Guard.IsNotNull(expectedTokenTexts);
            if (Is(expectedType))
            {
                for (int i = 0; i < expectedTokenTexts.Length; i++)
                {
                    if (IsTokenTextEqualTo(expectedTokenTexts[i], comparisonType))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool IsTokenTextEqualTo(string compareTo, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
        {
            if (string.IsNullOrEmpty(compareTo) || compareTo.Length != Length)
                return false;

            return _lineTextIncludingLineBreak.IndexOf(compareTo, StartInLine, Length, comparisonType) == StartInLine;
        }

        public string GetText()
        {
            return _lineTextIncludingLineBreak.Substring(StartInLine, Length);
        }

        public string GetText(int startInLine, int endInLine)
        {
            return _lineTextIncludingLineBreak.Substring(startInLine, endInLine - startInLine);
        }

        public override string ToString()
        {
            return $"[{Type}] ({StartInLine}, {EndInLine}): '{GetText()}'";
        }
    }
}
