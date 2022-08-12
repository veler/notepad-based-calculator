namespace NotepadBasedCalculator.Api
{
    public static class PredefinedTokenAndDataTypeNames
    {
        public const string UnsupportedCharacter = nameof(UnsupportedCharacter);

        public const string Whitespace = nameof(Whitespace);

        public const string SymbolOrPunctuation = nameof(SymbolOrPunctuation);

        public const string Digit = nameof(Digit);

        public const string Word = nameof(Word);

        public const string NewLine = nameof(NewLine);

        public const string Numeric = nameof(Numeric);

        public static class SubDataTypeNames
        {
            public const string Decimal = nameof(Decimal);

            public const string Integer = nameof(Integer);

            public const string Percentage = nameof(Percentage);

            public const string Ordinal = nameof(Ordinal);

            public const string Fraction = nameof(Fraction);
        }
    }
}
