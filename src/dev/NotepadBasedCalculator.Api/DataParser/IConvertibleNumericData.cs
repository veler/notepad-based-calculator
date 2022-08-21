namespace NotepadBasedCalculator.Api
{
    public interface IConvertibleNumericData : INumericData
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="types"></param>
        /// <returns>Returns <see cref="null"/> if the conversion is not supported.</returns>
        INumericData? ConvertTo(string[] types);
    }
}
