namespace NotepadBasedCalculator.Api
{
    /// <summary>
    /// Indicates that the data can perform arithmeric operation with several type of data (not only the current one)
    /// and will handle incompatibility itself.
    /// </summary>
    public interface ISupportMultipleDataTypeForArithmeticOperation : INumericData
    {
    }
}
