using System.Runtime.CompilerServices;

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

            if (leftData is not null)
            {
                return PerformOperation(leftData, binaryOperatorExpression.Operator, rightData);
            }

            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IData? PerformOperation(IData? leftData, BinaryOperatorType binaryOperatorType, IData? rightData)
        {
            if (leftData is not null)
            {
                switch (binaryOperatorType)
                {
                    case BinaryOperatorType.Equality:
                    case BinaryOperatorType.NoEquality:
                    case BinaryOperatorType.LessThan:
                    case BinaryOperatorType.LessThanOrEqualTo:
                    case BinaryOperatorType.GreaterThan:
                    case BinaryOperatorType.GreaterThanOrEqualTo:
                        return BinaryOperation(leftData, binaryOperatorType, rightData);

                    case BinaryOperatorType.Addition:
                    case BinaryOperatorType.Subtraction:
                    case BinaryOperatorType.Multiply:
                    case BinaryOperatorType.Division:
                        return AlgebraOperation(leftData, binaryOperatorType, rightData);

                    default:
                        ThrowHelper.ThrowNotSupportedException();
                        break;
                }
            }

            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IData? BinaryOperation(IData leftData, BinaryOperatorType binaryOperatorType, IData? rightData)
        {
            if (rightData is null)
            {
                return leftData;
            }

            if (leftData is not INumericData leftNumericData
                || rightData is not INumericData rightNumericData)
            {
                return null;
            }

            bool result;

            switch (binaryOperatorType)
            {
                case BinaryOperatorType.Equality:
                    result
                        = leftNumericData.ToStandardUnit().NumericValue
                        == rightNumericData.ToStandardUnit().NumericValue;
                    break;

                case BinaryOperatorType.NoEquality:
                    result
                        = leftNumericData.ToStandardUnit().NumericValue
                        != rightNumericData.ToStandardUnit().NumericValue;
                    break;

                case BinaryOperatorType.LessThan:
                    result
                        = leftNumericData.ToStandardUnit().NumericValue
                        < rightNumericData.ToStandardUnit().NumericValue;
                    break;

                case BinaryOperatorType.LessThanOrEqualTo:
                    result
                        = leftNumericData.ToStandardUnit().NumericValue
                        <= rightNumericData.ToStandardUnit().NumericValue;
                    break;

                case BinaryOperatorType.GreaterThan:
                    result
                        = leftNumericData.ToStandardUnit().NumericValue
                        > rightNumericData.ToStandardUnit().NumericValue;
                    break;

                case BinaryOperatorType.GreaterThanOrEqualTo:
                    result
                        = leftNumericData.ToStandardUnit().NumericValue
                        >= rightNumericData.ToStandardUnit().NumericValue;
                    break;

                default:
                    ThrowHelper.ThrowNotSupportedException();
                    return null;
            }

            return new BooleanData(
                leftData.LineTextIncludingLineBreak,
                leftData.StartInLine,
                rightData.EndInLine,
                result);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IData? AlgebraOperation(IData leftData, BinaryOperatorType binaryOperatorType, IData? rightData)
        {
            if (rightData is null)
            {
                return leftData;
            }

            if (leftData is not INumericData leftNumericData
                || rightData is not INumericData rightNumericData)
            {
                return null;
            }

            float result;
            switch (binaryOperatorType)
            {
                case BinaryOperatorType.Addition:
                    result
                        = leftNumericData.ToStandardUnit().NumericValue
                        + rightNumericData.ToStandardUnit().GetNumericValueToRelativeTo(leftNumericData);
                    break;

                case BinaryOperatorType.Subtraction:
                    result
                        = leftNumericData.ToStandardUnit().NumericValue
                        - rightNumericData.ToStandardUnit().GetNumericValueToRelativeTo(leftNumericData);
                    break;

                case BinaryOperatorType.Multiply:
                    result
                        = leftNumericData.ToStandardUnit().NumericValue
                        * rightNumericData.ToStandardUnit().NumericValue;
                    break;

                case BinaryOperatorType.Division:
                    result
                        = leftNumericData.ToStandardUnit().NumericValue
                        / rightNumericData.ToStandardUnit().NumericValue;
                    break;

                default:
                    ThrowHelper.ThrowNotSupportedException();
                    return null;
            }

            if (leftData is not IConvertibleNumericData && rightData is IConvertibleNumericData)
            {
                return rightNumericData.FromStandardUnit(result).MergeDataLocations(leftNumericData);
            }
            else
            {
                return leftNumericData.FromStandardUnit(result).MergeDataLocations(rightNumericData);
            }
        }
    }
}
