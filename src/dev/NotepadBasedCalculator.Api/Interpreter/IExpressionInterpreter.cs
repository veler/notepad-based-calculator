namespace NotepadBasedCalculator.Api
{
    public interface IExpressionInterpreter
    {
        Task<IData?> InterpretExpressionAsync(
            string culture,
            IVariableService variableService,
            IExpressionInterpreter expressionInterpreter,
            Expression expression,
            CancellationToken cancellationToken);
    }
}
