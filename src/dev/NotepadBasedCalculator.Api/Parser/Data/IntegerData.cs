namespace NotepadBasedCalculator.Api
{
    public sealed record IntegerData : Data<int>
    {
        public IntegerData(int startInLine, string originalText, int value)
            : base(startInLine, originalText, value)
        {
        }
    }
}
