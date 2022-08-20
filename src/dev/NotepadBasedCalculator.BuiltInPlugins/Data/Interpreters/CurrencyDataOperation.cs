using System.Runtime.CompilerServices;

namespace NotepadBasedCalculator.BuiltInPlugins.Data.Interpreters
{
    [Export(typeof(IDataBinaryOperationInterpreter))]
    [SupportedDataType(typeof(CurrencyData))]
    internal sealed class CurrencyDataOperation : IDataBinaryOperationInterpreter
    {
        public IData? PerformOperation(IData? leftData, BinaryOperatorType binaryOperatorType, IData? rightData)
        {
            if (leftData is null || leftData is not CurrencyData currencyData)
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
                    return BinaryOperation(currencyData, binaryOperatorType, rightData);

                case BinaryOperatorType.Addition:
                case BinaryOperatorType.Subtraction:
                case BinaryOperatorType.Multiply:
                case BinaryOperatorType.Division:
                    return AlgebraOperation(currencyData, binaryOperatorType, rightData);

                default:
                    ThrowHelper.ThrowNotSupportedException();
                    break;
            }

            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IData? BinaryOperation(CurrencyData leftData, BinaryOperatorType binaryOperatorType, IData? rightData)
        {
            if (rightData is null)
            {
                return leftData;
            }

            float currencyValue = leftData.Value.Value;
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
        private static IData? AlgebraOperation(CurrencyData leftData, BinaryOperatorType binaryOperatorType, IData? rightData)
        {
            if (rightData is null)
            {
                return leftData;
            }

            float currencyValue = leftData.Value.Value;

            switch (binaryOperatorType)
            {
                case BinaryOperatorType.Addition:
                    currencyValue.Add(rightData);
                    break;

                case BinaryOperatorType.Subtraction:
                    currencyValue.Substract(rightData);
                    break;

                case BinaryOperatorType.Multiply:
                    currencyValue.Multiply(rightData);
                    break;

                case BinaryOperatorType.Division:
                    currencyValue.Divide(rightData);
                    break;

                default:
                    ThrowHelper.ThrowNotSupportedException();
                    return null;
            }

            return new CurrencyData(
                leftData.LineTextIncludingLineBreak,
                leftData.StartInLine,
                rightData.EndInLine,
                new CurrencyValue(
                    currencyValue,
                    leftData.Value.Currency,
                    leftData.Value.IsoCurrency));
        }
    }
}
