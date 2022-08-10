namespace NotepadBasedCalculator.Api
{
    public sealed record PercentageData : Data<float>
    {
        public PercentageData(int startInLine, string originalText, float value)
            : base(startInLine, originalText, value)
        {
        }
    }
}
