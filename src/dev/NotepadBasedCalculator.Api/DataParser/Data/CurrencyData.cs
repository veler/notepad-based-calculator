namespace NotepadBasedCalculator.Api
{
    public sealed record CurrencyData : Data<CurrencyValue>, INumericData
    {
        private readonly Lazy<string> _displayText;

        public bool IsNegative => Value.Value < 0;

        public override string DisplayText => _displayText.Value;

        public CurrencyData(string lineTextIncludingLineBreak, int startInLine, int endInLine, CurrencyValue value)
            : base(
                  lineTextIncludingLineBreak,
                  startInLine,
                  endInLine,
                  value,
                  PredefinedTokenAndDataTypeNames.Numeric,
                  PredefinedTokenAndDataTypeNames.SubDataTypeNames.Currency)
        {
            _displayText = new Lazy<string>(() =>
            {
                if (!string.IsNullOrWhiteSpace(Value.IsoCurrency))
                {
                    return $"{Value.Value} {Value.IsoCurrency}";
                }

                return $"{Value.Value} {Value.Currency}";
            });
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
