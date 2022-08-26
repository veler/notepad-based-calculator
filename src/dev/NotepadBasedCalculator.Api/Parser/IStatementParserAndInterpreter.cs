namespace NotepadBasedCalculator.Api
{
    public interface IStatementParserAndInterpreter
    {
        IParserAndInterpreterService ParserAndInterpreterService { get; }

        Task<bool> TryParseAndInterpretStatement(
            string culture,
            LinkedToken currentToken,
            IVariableService variableService,
            Ref<IData?> interpreterResult,
            CancellationToken cancellationToken);
    }
}
