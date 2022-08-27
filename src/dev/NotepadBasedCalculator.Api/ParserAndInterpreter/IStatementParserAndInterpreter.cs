namespace NotepadBasedCalculator.Api
{
    public interface IStatementParserAndInterpreter
    {
        IParserAndInterpreterService ParserAndInterpreterService { get; }

        Task<bool> TryParseAndInterpretStatementAsync(
            string culture,
            LinkedToken currentToken,
            IVariableService variableService,
            StatementParserAndInterpreterResult result,
            CancellationToken cancellationToken);
    }
}
