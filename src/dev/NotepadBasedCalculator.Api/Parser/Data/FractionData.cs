namespace NotepadBasedCalculator.Api
{
    public sealed record FractionData : Data<float>
    {
        public FractionData(int startInLine, string originalText, float value)
            : base(startInLine, originalText, value)
        {
        }
    }
}
