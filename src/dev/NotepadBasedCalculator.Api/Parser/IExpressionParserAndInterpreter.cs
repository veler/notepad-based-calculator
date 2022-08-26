namespace NotepadBasedCalculator.Api
{
    public interface IExpressionParserAndInterpreter
    {
        IParserAndInterpreterService ParserAndInterpreterService { get; }

        Task<bool> TryParseAndInterpretExpression(
            string culture,
            LinkedToken currentToken,
            IExpressionParserAndInterpreter expressionParserAndInterpreter,
            IVariableService variableService,
            Ref<IData?> interpreterResult,
            CancellationToken cancellationToken);
    }
}
