namespace NotepadBasedCalculator.Api
{
    public interface IReadOnylyVariableService
    {
        IData? GetVariableValue(string variableName);
    }
}
