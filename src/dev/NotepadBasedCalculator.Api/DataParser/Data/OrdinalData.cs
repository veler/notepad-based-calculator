namespace NotepadBasedCalculator.Api
{
    public sealed record OrdinalData : Data<long>, INumericData
    {
        public bool IsNegative => Value < 0;

        public float NumericValue => Value;

        public override string DisplayText => Value.ToString(); // TODO: Show "th", "st", "rd"...

        public OrdinalData(string lineTextIncludingLineBreak, int startInLine, int endInLine, long value)
            : base(
                  lineTextIncludingLineBreak,
                  startInLine,
                  endInLine,
                  value,
                  PredefinedTokenAndDataTypeNames.Numeric,
                  PredefinedTokenAndDataTypeNames.SubDataTypeNames.Ordinal)
        {
        }

        public override IData MergeDataLocations(IData otherData)
        {
            return new OrdinalData(LineTextIncludingLineBreak, StartInLine, otherData.EndInLine, Value);
        }

        public float GetNumericValueToRelativeTo(INumericData? relativeData)
        {
            throw new NotImplementedException();
        }

        public INumericData ToStandardUnit()
        {
            throw new NotImplementedException();
        }

        public INumericData FromStandardUnit(float newStandardUnitValue)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
