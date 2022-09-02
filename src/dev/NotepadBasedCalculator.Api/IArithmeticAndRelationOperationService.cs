namespace NotepadBasedCalculator.Api
{
    public interface IArithmeticAndRelationOperationService
    {
        IData? PerformOperation(IData? leftData, BinaryOperatorType binaryOperatorType, IData? rightData);

        IData? PerformBinaryOperation(IData? leftData, BinaryOperatorType binaryOperatorType, IData? rightData);

        IData? PerformAlgebraOperation(IData? leftData, BinaryOperatorType binaryOperatorType, IData? rightData);
    }
}
