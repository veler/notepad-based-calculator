namespace NotepadBasedCalculator.Api
{
    public interface INumericData : IData
    {
        bool IsNegative { get; }

        double NumericValue { get; }

        double GetNumericValueToRelativeTo(INumericData? relativeData);

        INumericData ToStandardUnit();

        INumericData FromStandardUnit(double newStandardUnitValue);
    }
}
