namespace NotepadBasedCalculator.Api
{
    public sealed record DurationData : Data<TimeSpan>, IConvertibleNumericData
    {
        public bool IsNegative => Value.Ticks < 0;

        public double NumericValueInCurrentUnit => Value.Ticks;

        public override string DisplayText => $"{Value}"; // TODO => Localize

        public DurationData(string lineTextIncludingLineBreak, int startInLine, int endInLine, TimeSpan value)
            : base(
                  lineTextIncludingLineBreak,
                  startInLine,
                  endInLine,
                  value,
                  PredefinedTokenAndDataTypeNames.Numeric,
                  PredefinedTokenAndDataTypeNames.SubDataTypeNames.Duration)
        {
        }

        public override IData MergeDataLocations(IData otherData)
        {
            return new DurationData(
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
