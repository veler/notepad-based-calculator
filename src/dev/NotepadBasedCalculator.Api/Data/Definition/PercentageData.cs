namespace NotepadBasedCalculator.Api
{
    public sealed record PercentageData : Data<double>, INumericData
    {
        public bool IsNegative => Value < 0;

        public double NumericValueInCurrentUnit => Value;

        public override string DisplayText => $"{Math.Round(Value, 2)}"; // TODO => Localize

        public PercentageData(string lineTextIncludingLineBreak, int startInLine, int endInLine, double value)
            : base(
                  lineTextIncludingLineBreak,
                  startInLine,
                  endInLine,
                  value,
                  PredefinedTokenAndDataTypeNames.Numeric,
                  PredefinedTokenAndDataTypeNames.SubDataTypeNames.Percentage)
        {
        }

        public override IData MergeDataLocations(IData otherData)
        {
            return new PercentageData(
                LineTextIncludingLineBreak,
                Math.Min(StartInLine, otherData.StartInLine),
                Math.Max(EndInLine, otherData.EndInLine),
                Value);
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
