namespace NotepadBasedCalculator.BuiltInPlugins.Data.Interpreters
{
    [Export(typeof(IDataBinaryOperationInterpreter))]
    [SupportedDataType(typeof(IntegerData))]
    internal sealed class IntegerDataOperation : IDataBinaryOperationInterpreter
    {
        public IData? PerformOperation(IData? leftData, BinaryOperatorType binaryOperatorType, IData? rightData)
        {
            if (leftData is null || leftData is not IntegerData integerData)
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
                    return Addition(integerData, rightData);

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

        private IData? Addition(IntegerData leftData, IData? rightData)
        {
            return null;
        }
    }
}
