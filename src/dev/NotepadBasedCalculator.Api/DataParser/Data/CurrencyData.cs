namespace NotepadBasedCalculator.Api
{
    public sealed record CurrencyData : Data<CurrencyValue>, INumericData, IConvertibleNumericData
    {
        private readonly Lazy<string> _displayText;

        public bool IsNegative => Value.Value < 0;

        public float NumericValue => Value.Value;

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
                // TODO => Localize.
                if (!string.IsNullOrWhiteSpace(Value.IsoCurrency))
                {
                    return $"{Value.Value} {Value.IsoCurrency}";
                }

                return $"{Value.Value} {Value.Currency}";
            });
        }

        public override IData MergeDataLocations(IData otherData)
        {
            return new CurrencyData(
                LineTextIncludingLineBreak,
                Math.Min(StartInLine, otherData.StartInLine),
                Math.Max(EndInLine, otherData.EndInLine),
                Value);
        }

        public float GetNumericValueToRelativeTo(INumericData? relativeData)
        {
            return NumericValue;
        }

        public INumericData ToStandardUnit()
        {
            // TODO: Convert to USD.
            return new CurrencyData(LineTextIncludingLineBreak, StartInLine, EndInLine, Value);
        }

        public INumericData FromStandardUnit(float newStandardUnitValue)
        {
            // TODO: Convert newStandardUnitValue from USD to the currency defined in the current instance.
            return new CurrencyData(
                LineTextIncludingLineBreak,
                StartInLine,
                EndInLine,
                new CurrencyValue(
                    newStandardUnitValue,
                    this.Value.Currency,
                    this.Value.IsoCurrency));
        }

        public INumericData? ConvertTo(string[] types)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
