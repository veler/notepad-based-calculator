namespace NotepadBasedCalculator.Api
{
    public interface IParserAndInterpreterService
    {
        Task<bool> TryParseAndInterpretExpression(
            string culture,
            LinkedToken currentToken,
            IVariableService variableService,
            Ref<IData?> interpreterResult,
            CancellationToken cancellationToken);

        Task<bool> TryParseAndInterpretExpression(
            string expressionParserName,
            string culture,
            LinkedToken currentToken,
            IVariableService variableService,
            Ref<IData?> interpreterResult,
            CancellationToken cancellationToken);

        Task<bool> TryParseAndInterpretExpression(
            string culture,
            LinkedToken currentToken,
            string? parseUntilTokenIsOfType,
            string? parseUntilTokenHasText,
            IVariableService variableService,
            Ref<IData?> interpreterResult,
            CancellationToken cancellationToken);
    }
}
