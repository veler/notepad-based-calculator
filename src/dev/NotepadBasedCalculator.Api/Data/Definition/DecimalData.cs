namespace NotepadBasedCalculator.Api
{
    public sealed record DecimalData : Data<double>, INumericData
    {
        public bool IsNegative => Value < 0;

        public double NumericValue => Value;

        public override string DisplayText => Value.ToString(); // TODO => Localize. For example, in french, double separator is `,` instead of `.`

        public DecimalData(string lineTextIncludingLineBreak, int startInLine, int endInLine, double value)
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
            return new DecimalData(
                LineTextIncludingLineBreak,
                Math.Min(StartInLine, otherData.StartInLine),
                Math.Max(EndInLine, otherData.EndInLine),
                Value);
        }

        public double GetNumericValueToRelativeTo(INumericData? relativeData)
        {
            return NumericValue;
        }

        public INumericData ToStandardUnit()
        {
            return this;
        }

        public INumericData FromStandardUnit(double newStandardUnitValue)
        {
            return new DecimalData(LineTextIncludingLineBreak, StartInLine, EndInLine, newStandardUnitValue);
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
