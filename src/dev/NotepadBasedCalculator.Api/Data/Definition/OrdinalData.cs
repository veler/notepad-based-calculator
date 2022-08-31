namespace NotepadBasedCalculator.Api
{
    public sealed record OrdinalData : Data<long>, INumericData
    {
        public bool IsNegative => Value < 0;

        public double NumericValueInCurrentUnit => Value;

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
            return new OrdinalData(
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
