namespace NotepadBasedCalculator.Api
{
    public sealed record OrdinalData : Data<long>
    {
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
    }
}
