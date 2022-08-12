namespace NotepadBasedCalculator.Api
{
    public sealed record IntegerData : Data<long>
    {
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
    }
}
