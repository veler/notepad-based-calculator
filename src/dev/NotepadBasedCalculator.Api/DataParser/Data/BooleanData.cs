namespace NotepadBasedCalculator.Api
{
    public sealed record BooleanData : Data<bool>
    {
        public override string DisplayText => Value.ToString(); // TODO => Localize. For example, in french, decimal separator is `,` instead of `.`

        public BooleanData(string lineTextIncludingLineBreak, int startInLine, int endInLine, bool value)
            : base(
                  lineTextIncludingLineBreak,
                  startInLine,
                  endInLine,
                  value,
                  PredefinedTokenAndDataTypeNames.Numeric,
                  PredefinedTokenAndDataTypeNames.SubDataTypeNames.Decimal)
        {
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
