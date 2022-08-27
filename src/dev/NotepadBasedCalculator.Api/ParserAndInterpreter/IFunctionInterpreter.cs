namespace NotepadBasedCalculator.Api
{
    public interface IFunctionInterpreter
    {
        Task<IData?> InterpretFunctionAsync(
            string culture,
            FunctionDefinition functionDefinition,
            IReadOnlyList<IData> detectedData,
            CancellationToken cancellationToken);
    }
}
