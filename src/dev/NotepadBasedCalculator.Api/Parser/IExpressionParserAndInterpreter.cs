namespace NotepadBasedCalculator.Api
{
    public interface IExpressionParserAndInterpreter
    {
        IParserAndInterpreterService ParserAndInterpreterService { get; }

        Task<bool> TryParseAndInterpretExpressionAsync(
            string culture,
            LinkedToken currentToken,
            IVariableService variableService,
            ExpressionParserAndInterpreterResult result,
            CancellationToken cancellationToken);
    }
}
