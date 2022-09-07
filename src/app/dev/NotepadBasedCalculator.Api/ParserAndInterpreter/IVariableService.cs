namespace NotepadBasedCalculator.Api
{
    public interface IVariableService
    {
        void SetVariableValue(string variableName, IData? value);

        IData? GetVariableValue(string variableName);
    }
}
