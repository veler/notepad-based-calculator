namespace NotepadBasedCalculator.Api
{
    public interface ICurrencyService
    {
        Task<double?> ConvertCurrencyAsync(string fromCurrencyIso, double value, string toCurrencyIso);
    }
}
