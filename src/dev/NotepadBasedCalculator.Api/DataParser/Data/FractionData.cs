namespace NotepadBasedCalculator.Api
{
    public sealed record FractionData : Data<float>, INumericData
    {
        public bool IsNegative => Value < 0;

        public override string DisplayText => Value.ToString(); // TODO => Localize

        public FractionData(string lineTextIncludingLineBreak, int startInLine, int endInLine, float value)
            : base(
                  lineTextIncludingLineBreak,
                  startInLine,
                  endInLine,
                  value,
                  PredefinedTokenAndDataTypeNames.Numeric,
                  PredefinedTokenAndDataTypeNames.SubDataTypeNames.Fraction)
        {
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
