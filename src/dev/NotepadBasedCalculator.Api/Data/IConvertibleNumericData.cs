namespace NotepadBasedCalculator.Api
{
    public interface IConvertibleNumericData : INumericData
    {
        /// <summary>
        /// Creates a new instance of <see cref="IConvertibleNumericData"/> with the same
        /// units than the current instance, but uses <paramref name="from"/> as original value.
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        INumericData? ConvertFrom(INumericData from);

        bool CanConvertFrom(INumericData from);
    }
}
