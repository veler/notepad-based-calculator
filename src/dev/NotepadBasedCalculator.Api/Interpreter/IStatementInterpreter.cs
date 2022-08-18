namespace NotepadBasedCalculator.Api
{
    public interface IStatementInterpreter
    {
        Task<IData?> InterpretStatementAsync(
            string culture,
            IVariableService variableService,
            IExpressionInterpreter expressionInterpreter,
            Statement statement,
            CancellationToken cancellationToken);
    }
}
