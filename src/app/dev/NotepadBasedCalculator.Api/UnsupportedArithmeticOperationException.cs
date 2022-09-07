namespace NotepadBasedCalculator.Api
{
    public class UnsupportedArithmeticOperationException : DataOperationException
    {
        public override string GetLocalizedMessage(string culture)
        {
            // TODO: Localize.
            return "Unsupported arithmetic operation";
        }
    }
}
