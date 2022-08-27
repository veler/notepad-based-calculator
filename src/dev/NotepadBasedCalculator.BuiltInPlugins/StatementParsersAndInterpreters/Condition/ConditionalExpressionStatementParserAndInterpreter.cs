using NotepadBasedCalculator.BuiltInPlugins.StatementParsersAndInterpreters.NumericalExpression;

namespace NotepadBasedCalculator.BuiltInPlugins.StatementParsersAndInterpreters.Condition
{
    [Export(typeof(IStatementParserAndInterpreter))]
    [Culture(SupportedCultures.English)]
    [Shared]
    internal sealed class ConditionalExpressionStatementParserAndInterpreter : IStatementParserAndInterpreter
    {
        [Import]
        public IParserAndInterpreterService ParserAndInterpreterService { get; set; } = null!;

        public async Task<bool> TryParseAndInterpretStatementAsync(
            string culture,
            LinkedToken currentToken,
            IVariableService variableService,
            StatementParserAndInterpreterResult result,
            CancellationToken cancellationToken)
        {
            var expressionResult = new ExpressionParserAndInterpreterResult();

            bool expressionFound
                = await ParserAndInterpreterService.TryParseAndInterpretExpressionAsync(
                    PredefinedExpressionParserNames.ConditionalExpression,
                    culture,
                    currentToken,
                    variableService,
                    expressionResult,
                    cancellationToken);

            if (expressionFound)
            {
                if (expressionResult.ParsedExpression is BinaryOperatorExpression binaryOperatorExpression
                    && binaryOperatorExpression.Operator
                        is BinaryOperatorType.NoEquality
                        or BinaryOperatorType.Equality
                        or BinaryOperatorType.LessThanOrEqualTo
                        or BinaryOperatorType.LessThan
                        or BinaryOperatorType.GreaterThanOrEqualTo
                        or BinaryOperatorType.GreaterThan)
                {
                    result.ParsedStatement
                        = new ConditionStatement(
                            expressionResult.ParsedExpression!.FirstToken,
                            expressionResult.ParsedExpression.LastToken,
                            expressionResult.ParsedExpression,
                            expressionResult.ResultedData);
                    result.ResultedData = expressionResult.ResultedData;
                    return true;
                }

                // Fast path
                if (expressionFound
                    && expressionResult.ParsedExpression
                        is DataExpression
                        or VariableReferenceExpression
                        or GroupExpression
                        or BinaryOperatorExpression)
                {
                    if (expressionResult.ParsedExpression is BinaryOperatorExpression binaryOperatorExpression2
                        && !(binaryOperatorExpression2.Operator
                            is BinaryOperatorType.Addition
                            or BinaryOperatorType.Division
                            or BinaryOperatorType.Multiply
                            or BinaryOperatorType.Subtraction))
                    {
                        return false;
                    }

                    result.ParsedStatement
                        = new NumericalCalculusStatement(
                            expressionResult.ParsedExpression!.FirstToken,
                            expressionResult.ParsedExpression.LastToken,
                            expressionResult.ParsedExpression);
                    result.ResultedData = expressionResult.ResultedData;
                    return true;
                }
            }

            return false;
        }
    }
}
