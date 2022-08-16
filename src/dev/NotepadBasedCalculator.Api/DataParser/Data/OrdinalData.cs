namespace NotepadBasedCalculator.Api
{
    public sealed record OrdinalData : Data<long>, INumericData
    {
        public bool IsNegative => Value < 0;

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

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
