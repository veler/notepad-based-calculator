namespace NotepadBasedCalculator.Api
{
    public struct CurrencyValue
    {
        public CurrencyValue(float value, string currency, string isoCurrency)
        {
            Value = value;
            Currency = currency;
            IsoCurrency = isoCurrency;
        }

        public float Value { get; }

        public string Currency { get; }

        public string IsoCurrency { get; }
    }
}
