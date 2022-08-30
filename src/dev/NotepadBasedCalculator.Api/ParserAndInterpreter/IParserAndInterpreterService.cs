namespace NotepadBasedCalculator.Api
{
    public interface IParserAndInterpreterService
    {
        Task<bool> TryParseAndInterpretExpressionAsync(
            string culture,
            LinkedToken? currentToken,
            IVariableService variableService,
            ExpressionParserAndInterpreterResult result,
            CancellationToken cancellationToken);

        Task<bool> TryParseAndInterpretExpressionAsync(
            string expressionParserAndInterpreterName,
            string culture,
            LinkedToken? currentToken,
            IVariableService variableService,
            ExpressionParserAndInterpreterResult result,
            CancellationToken cancellationToken);

        Task<bool> TryParseAndInterpretExpressionAsync(
            string culture,
            LinkedToken? currentToken,
            string? parseUntilTokenIsOfType,
            string? parseUntilTokenHasText,
            IVariableService variableService,
            ExpressionParserAndInterpreterResult result,
            CancellationToken cancellationToken);

        Task<bool> TryParseAndInterpretExpressionAsync(
            string culture,
            LinkedToken? currentToken,
            string? parseUntilTokenIsOfType,
            string? parseUntilTokenHasText,
            string? nestedTokenType,
            IVariableService variableService,
            ExpressionParserAndInterpreterResult result,
            CancellationToken cancellationToken);

        Task<bool> TryParseAndInterpretExpressionAsync(
            string expressionParserAndInterpreterName,
            string culture,
            LinkedToken? currentToken,
            string? parseUntilTokenIsOfType,
            string? parseUntilTokenHasText,
            IVariableService variableService,
            ExpressionParserAndInterpreterResult result,
            CancellationToken cancellationToken);

        Task<bool> TryParseAndInterpretExpressionAsync(
            string expressionParserAndInterpreterName,
            string culture,
            LinkedToken? currentToken,
            string? parseUntilTokenIsOfType,
            string? parseUntilTokenHasText,
            string? nestedTokenType,
            IVariableService variableService,
            ExpressionParserAndInterpreterResult result,
            CancellationToken cancellationToken);

        Task<bool> TryParseAndInterpretStatementAsync(
            string culture,
            LinkedToken? currentToken,
            string? parseUntilTokenIsOfType,
            string? parseUntilTokenHasText,
            IVariableService variableService,
            StatementParserAndInterpreterResult result,
            CancellationToken cancellationToken);
    }
}
