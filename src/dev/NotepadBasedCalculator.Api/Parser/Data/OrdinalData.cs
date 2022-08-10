namespace NotepadBasedCalculator.Api
{
    public sealed record OrdinalData : Data<int>
    {
        public OrdinalData(int startInLine, string originalText, int value)
            : base(startInLine, originalText, value)
        {
        }
    }
}
