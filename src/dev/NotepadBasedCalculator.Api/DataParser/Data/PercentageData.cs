namespace NotepadBasedCalculator.Api
{
    public sealed record PercentageData : Data<float>, INumericData
    {
        public bool IsNegative => Value < 0;

        public float NumericValue => Value;

        public override string DisplayText => $"{Value}"; // TODO => Localize

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

        public override IData MergeDataLocations(IData otherData)
        {
            return new PercentageData(LineTextIncludingLineBreak, StartInLine, otherData.EndInLine, Value);
        }

        public float GetNumericValueToRelativeTo(INumericData? relativeData)
        {
            float percentage = NumericValue;

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

        public INumericData FromStandardUnit(float newStandardUnitValue)
        {
            return new PercentageData(LineTextIncludingLineBreak, StartInLine, EndInLine, newStandardUnitValue);
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
