namespace NotepadBasedCalculator.Api
{
    public sealed record BooleanData : Data<bool>, INumericData
    {
        public override string DisplayText => Value.ToString(); // TODO => Localize. For example, in french, decimal separator is `,` instead of `.`

        public bool IsNegative => false;

        public float NumericValue => Convert.ToInt32(Value);

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
            return new BooleanData(LineTextIncludingLineBreak, StartInLine, otherData.EndInLine, Value);
        }

        public float GetNumericValueToRelativeTo(INumericData? relativeData)
        {
            return NumericValue;
        }

        public INumericData ToStandardUnit()
        {
            return this;
        }

        public INumericData FromStandardUnit(float newStandardUnitValue)
        {
            if (newStandardUnitValue != 0 && newStandardUnitValue != 1)
            {
                return new DecimalData(LineTextIncludingLineBreak, StartInLine, EndInLine, newStandardUnitValue);
            }

            return new BooleanData(LineTextIncludingLineBreak, StartInLine, EndInLine, newStandardUnitValue == 0 ? false : true);
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
