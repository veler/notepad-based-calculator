namespace NotepadBasedCalculator.Api
{
    public interface IFunctionDefinitionProvider
    {
        IReadOnlyList<Dictionary<string, Dictionary<string, string[]>>> LoadFunctionDefinition(string culture);
    }
}
