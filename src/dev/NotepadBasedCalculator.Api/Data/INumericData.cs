namespace NotepadBasedCalculator.Api
{
    public interface INumericData : IData
    {
        bool IsNegative { get; }

        double NumericValueInCurrentUnit { get; }

        double NumericValueInStandardUnit { get; }

        INumericData CreateFromStandardUnit(double valueInStandardUnit);

        INumericData CreateFromCurrentUnit(double valueInCurrentUnit);

        INumericData Add(INumericData otherData);

        INumericData Substract(INumericData otherData);

        INumericData Multiply(INumericData otherData);

        INumericData Divide(INumericData otherData);
    }
}
