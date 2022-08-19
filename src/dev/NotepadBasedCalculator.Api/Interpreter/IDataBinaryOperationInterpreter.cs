namespace NotepadBasedCalculator.Api
{
    public interface IDataBinaryOperationInterpreter
    {
        IData? PerformOperation(IData? leftData, BinaryOperatorType binaryOperatorType, IData? rightData);
    }
}
