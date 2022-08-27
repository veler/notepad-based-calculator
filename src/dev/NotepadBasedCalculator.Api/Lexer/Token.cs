namespace NotepadBasedCalculator.Api
{
    /// <summary>
    /// Represents a token.
    /// </summary>
    public record Token : IToken
    {
        private readonly Lazy<string> _getText;

        public string Type { get; }

        public int StartInLine { get; }

        public int EndInLine { get; }

        public int Length => EndInLine - StartInLine;

        public string LineTextIncludingLineBreak { get; }

        internal Token(string lineTextIncludingLineBreak, int startInLine, int endInLine, string tokenType)
        {
            Guard.IsGreaterThanOrEqualTo(startInLine, 0);
            Guard.IsGreaterThan(endInLine, startInLine);
            Guard.IsNotNullOrEmpty(lineTextIncludingLineBreak);
            Guard.IsNotNullOrWhiteSpace(tokenType);
            LineTextIncludingLineBreak = lineTextIncludingLineBreak;

            Type = tokenType;
            StartInLine = startInLine;
            EndInLine = endInLine;

            _getText = new Lazy<string>(() => LineTextIncludingLineBreak.Substring(StartInLine, Length));
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

            return LineTextIncludingLineBreak.IndexOf(compareTo, StartInLine, Length, comparisonType) == StartInLine;
        }

        public string GetText()
        {
            return _getText.Value;
        }

        public string GetText(int startInLine, int endInLine)
        {
            return LineTextIncludingLineBreak.Substring(startInLine, endInLine - startInLine);
        }

        public override string ToString()
        {
            return $"[{Type}] ({StartInLine}, {EndInLine}): '{GetText()}'";
        }
    }
}
