namespace NotepadBasedCalculator.Api
{
    public interface IValueRelativeToOtherData
    {
        double GetStandardUnitValueRelativeTo(INumericData other);
    }
}
