namespace NotepadBasedCalculator.Api
{
    public sealed record DecimalData : Data<float>
    {
        public DecimalData(int startInLine, string originalText, float value)
            : base(startInLine, originalText, value)
        {
        }
    }
}
