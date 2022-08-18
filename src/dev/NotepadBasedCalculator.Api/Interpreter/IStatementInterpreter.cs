namespace NotepadBasedCalculator.Api.Interpreter
{
    public interface IStatementInterpreter
    {
        Task<IData?> InterpretAsync(string culture, Statement statement, CancellationToken cancellationToken);
    }
}
