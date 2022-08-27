namespace NotepadBasedCalculator.Api
{
    public sealed record CurrencyData : Data<CurrencyValue>, INumericData, IConvertibleNumericData
    {
        private readonly Lazy<string> _displayText;

        public bool IsNegative => Value.Value < 0;

        public double NumericValue => Value.Value;

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

        public double GetNumericValueToRelativeTo(INumericData? relativeData)
        {
            return NumericValue;
        }

        public INumericData ToStandardUnit()
        {
            // TODO: Convert to USD.
            return new CurrencyData(LineTextIncludingLineBreak, StartInLine, EndInLine, Value);
        }

        public INumericData FromStandardUnit(double newStandardUnitValue)
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

        public INumericData? ConvertFrom(INumericData from)
        {
            // TODO

            return new CurrencyData(
                from.LineTextIncludingLineBreak,
                from.StartInLine,
                from.EndInLine,
                new CurrencyValue(
                    from.NumericValue,
                    this.Value.Currency,
                    this.Value.IsoCurrency));
        }

        public bool CanConvertFrom(INumericData from)
        {
            return from is DecimalData or CurrencyData;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
