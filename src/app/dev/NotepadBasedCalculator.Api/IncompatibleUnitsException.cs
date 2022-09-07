namespace NotepadBasedCalculator.Api
{
    public class IncompatibleUnitsException : DataOperationException
    {
        public override string GetLocalizedMessage(string culture)
        {
            // TODO: Localize.
            return "Incompatible units";
        }
    }
}
