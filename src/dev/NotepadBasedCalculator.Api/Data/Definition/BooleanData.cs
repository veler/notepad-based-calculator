namespace NotepadBasedCalculator.Api
{
    public sealed record BooleanData : Data<bool>, INumericData
    {
        public override string DisplayText => Value.ToString(); // TODO => Localize. For example, in french, double separator is `,` instead of `.`

        public bool IsNegative => false;

        public double NumericValueInCurrentUnit => Convert.ToInt32(Value);

        public BooleanData(string lineTextIncludingLineBreak, int startInLine, int endInLine, bool value)
            : base(
                  lineTextIncludingLineBreak,
                  startInLine,
                  endInLine,
                  value,
                  PredefinedTokenAndDataTypeNames.Numeric,
                  PredefinedTokenAndDataTypeNames.SubDataTypeNames.Decimal)
        {
        }

        public override IData MergeDataLocations(IData otherData)
        {
            return new BooleanData(
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
