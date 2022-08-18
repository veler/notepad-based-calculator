namespace NotepadBasedCalculator.Api.Interpreter
{
    public interface IExpressionInterpreter
    {
        Task<IData?> InterpretAsync(string culture, Expression expression, CancellationToken cancellationToken);
    }
}
