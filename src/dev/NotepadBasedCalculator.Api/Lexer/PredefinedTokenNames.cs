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

        public const string LeftParenth = nameof(LeftParenth);

        public const string RightParenth = nameof(RightParenth);

        public const string IsEqualToOperator = "equal_to_operators";

        public const string IsNotEqualToOperator = "not_equal_to_operators";

        public const string LessThanOrEqualToOperator = "less_than_or_equal_to_operators";

        public const string LessThanOperator = "less_than_operators";

        public const string GreaterThanOrEqualToOperator = "greater_than_or_equal_to_operators";

        public const string GreaterThanOperator = "greater_than_operators";

        public const string AdditionOperator = "addition_operators";

        public const string SubstractionOperator = "substration_operators";

        public const string MultiplicationOperator = "multiplication_operators";

        public const string DivisionOperator = "division_operators";

        public const string IfIdentifier = "if_identifiers";

        public const string ThenIdentifier = "then_identifiers";

        public const string ElseIdentifier = "else_identifiers";

        public const string CommentOperator = "comment_operators";

        public const string HeaderOperator = "header_operators";

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
