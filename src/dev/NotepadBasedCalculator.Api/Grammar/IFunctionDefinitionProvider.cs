namespace NotepadBasedCalculator.Api
{
    public interface IFunctionDefinitionProvider
    {
        IReadOnlyList<FunctionDefinition> LoadFunctionDefinition(string culture);
    }
}
