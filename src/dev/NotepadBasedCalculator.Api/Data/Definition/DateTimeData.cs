namespace NotepadBasedCalculator.Api
{
    public sealed record DateTimeData : Data<DateTime>, IConvertibleNumericData
    {
        public bool IsNegative => Value.Ticks < 0;

        public double NumericValueInCurrentUnit => Value.Ticks;

        public override string DisplayText => $"{Value}"; // TODO => Localize

        public DateTimeData(string lineTextIncludingLineBreak, int startInLine, int endInLine, DateTime value)
            : base(
                  lineTextIncludingLineBreak,
                  startInLine,
                  endInLine,
                  value,
                  PredefinedTokenAndDataTypeNames.Numeric,
                  PredefinedTokenAndDataTypeNames.SubDataTypeNames.DateTime)
        {
        }

        public override IData MergeDataLocations(IData otherData)
        {
            return new DateTimeData(
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
