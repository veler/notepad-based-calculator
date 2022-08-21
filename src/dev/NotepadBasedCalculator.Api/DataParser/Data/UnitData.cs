namespace NotepadBasedCalculator.Api
{
    public sealed record UnitData : Data<UnitFloat>, INumericData
    {
        public bool IsNegative => Value.Value < 0;

        public float NumericValue => Value.Value;

        public override string DisplayText => $"{Value.Value} {Value.Unit}"; // TODO => Localize

        public UnitData(string lineTextIncludingLineBreak, int startInLine, int endInLine, string subType, UnitFloat value)
            : base(
                  lineTextIncludingLineBreak,
                  startInLine,
                  endInLine,
                  value,
                  PredefinedTokenAndDataTypeNames.Numeric,
                  subType)
        {
        }

        public override IData MergeDataLocations(IData otherData)
        {
            return new UnitData(LineTextIncludingLineBreak, StartInLine, otherData.EndInLine, Subtype!, Value);
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
