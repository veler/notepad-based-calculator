namespace NotepadBasedCalculator.Api
{
    public interface INumericData : IData
    {
        bool IsNegative { get; }

        double NumericValueInCurrentUnit { get; }

        double NumericValueInStandardUnit { get; }

        INumericData CreateFromStandardUnit(double value);

        INumericData CreateFromCurrentUnit(double value);

        INumericData Substract(INumericData value);

        INumericData Add(INumericData value);

        INumericData Multiply(INumericData value);

        INumericData Divide(INumericData value);
    }
}
