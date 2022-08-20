namespace NotepadBasedCalculator.Api
{
    public sealed record DecimalData : Data<float>, INumericData
    {
        public bool IsNegative => Value < 0;

        public override string DisplayText => Value.ToString(); // TODO => Localize. For example, in french, decimal separator is `,` instead of `.`

        public DecimalData(string lineTextIncludingLineBreak, int startInLine, int endInLine, float value)
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
