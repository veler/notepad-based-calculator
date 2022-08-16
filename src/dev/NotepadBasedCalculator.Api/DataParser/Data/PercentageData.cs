namespace NotepadBasedCalculator.Api
{
    public sealed record PercentageData : Data<float>, INumericData
    {
        public bool IsNegative => Value < 0;

        public PercentageData(string lineTextIncludingLineBreak, int startInLine, int endInLine, float value)
            : base(
                  lineTextIncludingLineBreak,
                  startInLine,
                  endInLine,
                  value,
                  PredefinedTokenAndDataTypeNames.Numeric,
                  PredefinedTokenAndDataTypeNames.SubDataTypeNames.Percentage)
        {
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
