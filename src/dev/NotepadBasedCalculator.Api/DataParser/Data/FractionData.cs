namespace NotepadBasedCalculator.Api
{
    public sealed record FractionData : Data<float>, INumericData
    {
        public bool IsNegative => Value < 0;

        public float NumericValue => Value;

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

        public override IData MergeDataLocations(IData otherData)
        {
            return new FractionData(
                LineTextIncludingLineBreak,
                Math.Min(StartInLine, otherData.StartInLine),
                Math.Max(EndInLine, otherData.EndInLine),
                Value);
        }

        public float GetNumericValueToRelativeTo(INumericData? relativeData)
        {
            if (relativeData is null)
            {
                return NumericValue;
            }

            return relativeData.NumericValue * NumericValue;
        }

        public INumericData ToStandardUnit()
        {
            return this;
        }

        public INumericData FromStandardUnit(float newStandardUnitValue)
        {
            return new FractionData(LineTextIncludingLineBreak, StartInLine, EndInLine, newStandardUnitValue);
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
