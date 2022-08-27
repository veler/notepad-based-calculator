namespace NotepadBasedCalculator.Api
{
    public sealed record PercentageData : Data<double>, INumericData
    {
        public bool IsNegative => Value < 0;

        public double NumericValue => Value;

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

        public double GetNumericValueToRelativeTo(INumericData? relativeData)
        {
            double percentage = NumericValue;

            if (relativeData is null || relativeData is PercentageData)
            {
                return percentage;
            }
            else
            {
                return percentage * relativeData.NumericValue;
            }
        }

        public INumericData ToStandardUnit()
        {
            return this;
        }

        public INumericData FromStandardUnit(double newStandardUnitValue)
        {
            return new PercentageData(LineTextIncludingLineBreak, StartInLine, EndInLine, newStandardUnitValue);
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
