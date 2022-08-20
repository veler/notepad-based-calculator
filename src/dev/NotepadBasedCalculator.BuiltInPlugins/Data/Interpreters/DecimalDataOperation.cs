using System.Runtime.CompilerServices;

namespace NotepadBasedCalculator.BuiltInPlugins.Data.Interpreters
{
    [Export(typeof(IDataBinaryOperationInterpreter))]
    [SupportedDataType(typeof(DecimalData))]
    internal sealed class DecimalDataOperation : IDataBinaryOperationInterpreter
    {
        public IData? PerformOperation(IData? leftData, BinaryOperatorType binaryOperatorType, IData? rightData)
        {
            if (leftData is null || leftData is not DecimalData decimalData)
            {
                return null;
            }

            switch (binaryOperatorType)
            {
                case BinaryOperatorType.Equality:
                case BinaryOperatorType.NoEquality:
                case BinaryOperatorType.LessThan:
                case BinaryOperatorType.LessThanOrEqualTo:
                case BinaryOperatorType.GreaterThan:
                case BinaryOperatorType.GreaterThanOrEqualTo:
                    return BinaryOperation(decimalData, binaryOperatorType, rightData);

                case BinaryOperatorType.Addition:
                case BinaryOperatorType.Subtraction:
                case BinaryOperatorType.Multiply:
                case BinaryOperatorType.Division:
                    return AlgebraOperation(decimalData, binaryOperatorType, rightData);

                default:
                    ThrowHelper.ThrowNotSupportedException();
                    break;
            }

            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IData? BinaryOperation(DecimalData leftData, BinaryOperatorType binaryOperatorType, IData? rightData)
        {
            if (rightData is null)
            {
                return leftData;
            }

            float currencyValue = leftData.Value;
            bool result;

            switch (binaryOperatorType)
            {
                case BinaryOperatorType.Equality:
                    result = currencyValue.IsEqualTo(rightData);
                    break;

                case BinaryOperatorType.NoEquality:
                    result = !currencyValue.IsEqualTo(rightData);
                    break;

                case BinaryOperatorType.LessThan:
                    result = currencyValue.LessThan(rightData);
                    break;

                case BinaryOperatorType.LessThanOrEqualTo:
                    result = currencyValue.LessThanOrEqualTo(rightData);
                    break;

                case BinaryOperatorType.GreaterThan:
                    result = currencyValue.GreaterThan(rightData);
                    break;

                case BinaryOperatorType.GreaterThanOrEqualTo:
                    result = currencyValue.GreaterThanOrEqualTo(rightData);
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
        private static IData? AlgebraOperation(DecimalData leftData, BinaryOperatorType binaryOperatorType, IData? rightData)
        {
            if (rightData is null)
            {
                return leftData;
            }

            float value = leftData.Value;

            switch (binaryOperatorType)
            {
                case BinaryOperatorType.Addition:
                    value.Add(rightData);
                    break;

                case BinaryOperatorType.Subtraction:
                    value.Substract(rightData);
                    break;

                case BinaryOperatorType.Multiply:
                    value.Multiply(rightData);
                    break;

                case BinaryOperatorType.Division:
                    value.Divide(rightData);
                    break;

                default:
                    ThrowHelper.ThrowNotSupportedException();
                    return null;
            }

            return new DecimalData(
                leftData.LineTextIncludingLineBreak,
                leftData.StartInLine,
                rightData.EndInLine,
                value);
        }
    }
}
