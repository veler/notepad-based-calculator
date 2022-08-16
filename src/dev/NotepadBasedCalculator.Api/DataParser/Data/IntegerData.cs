namespace NotepadBasedCalculator.Api
{
    public sealed record IntegerData : Data<long>, INumericData
    {
        public bool IsNegative => Value < 0;

        public IntegerData(string lineTextIncludingLineBreak, int startInLine, int endInLine, long value)
            : base(
                  lineTextIncludingLineBreak,
                  startInLine,
                  endInLine,
                  value,
                  PredefinedTokenAndDataTypeNames.Numeric,
                  PredefinedTokenAndDataTypeNames.SubDataTypeNames.Integer)
        {
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
