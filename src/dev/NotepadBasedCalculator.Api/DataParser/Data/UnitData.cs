namespace NotepadBasedCalculator.Api
{
    public sealed record UnitData : Data<UnitFloat>, INumericData
    {
        public bool IsNegative => Value.Value < 0;

        public override string DisplayText => $"{Value.Value} {Value.Unit}";

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

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
