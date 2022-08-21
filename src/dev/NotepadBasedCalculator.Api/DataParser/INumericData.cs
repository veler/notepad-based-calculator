namespace NotepadBasedCalculator.Api
{
    public interface INumericData : IData
    {
        bool IsNegative { get; }

        float NumericValue { get; }

        float GetNumericValueToRelativeTo(INumericData? relativeData);

        INumericData ToStandardUnit();

        INumericData FromStandardUnit(float newStandardUnitValue);
    }
}
