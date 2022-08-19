using NotepadBasedCalculator.Api;

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
                    break;

                case BinaryOperatorType.NoEquality:
                    break;

                case BinaryOperatorType.LessThan:
                    break;

                case BinaryOperatorType.LessThanOrEqualTo:
                    break;

                case BinaryOperatorType.GreaterThan:
                    break;

                case BinaryOperatorType.GreaterThanOrEqualTo:
                    break;

                case BinaryOperatorType.Addition:
                    return Addition(currencyData, rightData);

                case BinaryOperatorType.Subtraction:
                    break;

                case BinaryOperatorType.Multiply:
                    break;

                case BinaryOperatorType.Division:
                    break;

                default:
                    ThrowHelper.ThrowNotSupportedException();
                    return null;
            }

            return null;
        }

        private static IData? Addition(CurrencyData leftData, IData? rightData)
        {
            if (rightData is null)
            {
                return leftData;
            }

            float currencyValue = leftData.Value.Value;
            currencyValue.Add(rightData);

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
