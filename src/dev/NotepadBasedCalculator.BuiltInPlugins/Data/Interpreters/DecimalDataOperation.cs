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
                    return Addition(decimalData, rightData);

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

        private IData? Addition(DecimalData leftData, IData? rightData)
        {
            return null;
        }
    }
}
