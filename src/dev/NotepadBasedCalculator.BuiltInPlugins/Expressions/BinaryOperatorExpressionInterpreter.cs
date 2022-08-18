namespace NotepadBasedCalculator.BuiltInPlugins.Expressions
{
    [Export(typeof(IExpressionInterpreter))]
    [SupportedExpressionType(typeof(BinaryOperatorExpression))]
    internal sealed class BinaryOperatorExpressionInterpreter : IExpressionInterpreter
    {
        public async Task<IData?> InterpretExpressionAsync(
            string culture,
            IVariableService variableService,
            IExpressionInterpreter expressionInterpreter,
            Expression expression,
            CancellationToken cancellationToken)
        {
            if (expression is not BinaryOperatorExpression binaryOperatorExpression)
            {
                return null;
            }

            IData? leftData
                = await expressionInterpreter.InterpretExpressionAsync(
                    culture,
                    variableService,
                    expressionInterpreter,
                    binaryOperatorExpression.LeftExpression,
                    cancellationToken)
                .ConfigureAwait(true);

            IData? rightData
                = await expressionInterpreter.InterpretExpressionAsync(
                    culture,
                    variableService,
                    expressionInterpreter,
                    binaryOperatorExpression.RightExpression,
                    cancellationToken)
                .ConfigureAwait(true);

            switch (binaryOperatorExpression.Operator)
            {
                case BinaryOperatorType.Equality:
                    return PerformEquality(leftData, rightData, notEqual: false);

                case BinaryOperatorType.NoEquality:
                    return PerformEquality(leftData, rightData, notEqual: true);

                case BinaryOperatorType.LessThan:
                case BinaryOperatorType.LessThanOrEqualTo:
                case BinaryOperatorType.GreaterThan:
                case BinaryOperatorType.GreaterThanOrEqualTo:
                    return PerformRelationalComparison(leftData, rightData, binaryOperatorExpression.Operator);

                case BinaryOperatorType.Addition:
                    break;

                case BinaryOperatorType.Subtraction:
                    break;

                case BinaryOperatorType.Multiply:
                    break;

                case BinaryOperatorType.Division:
                    break;

                default:
                    ThrowHelper.ThrowNotSupportedException();
                    break;
            }

            return null;
        }

        private IData? PerformEquality(IData? left, IData? right, bool notEqual)
        {
            return null;
        }

        private IData? PerformRelationalComparison(IData? left, IData? right, BinaryOperatorType operatorType)
        {
            return null;
        }
    }
}
