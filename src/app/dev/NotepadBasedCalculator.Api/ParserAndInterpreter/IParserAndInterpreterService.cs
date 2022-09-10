namespace NotepadBasedCalculator.Api
{
    public interface IParserAndInterpreterService
    {
        IArithmeticAndRelationOperationService ArithmeticAndRelationOperationService { get; }

        Task<bool> TryParseAndInterpretExpressionAsync(
            string culture,
            LinkedToken? currentToken,
            IVariableService variableService,
            ExpressionParserAndInterpreterResult result,
            CancellationToken cancellationToken);

        Task<bool> TryParseAndInterpretExpressionAsync(
            string[] expressionParserAndInterpreterNames,
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
            string[] expressionParserAndInterpreterNames,
            string culture,
            LinkedToken? currentToken,
            string? parseUntilTokenIsOfType,
            string? parseUntilTokenHasText,
            IVariableService variableService,
            ExpressionParserAndInterpreterResult result,
            CancellationToken cancellationToken);

        Task<bool> TryParseAndInterpretExpressionAsync(
            string[] expressionParserAndInterpreterNames,
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
