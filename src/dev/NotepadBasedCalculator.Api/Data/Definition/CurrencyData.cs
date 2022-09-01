using System.Globalization;

namespace NotepadBasedCalculator.Api
{
    public sealed record CurrencyData : Data<CurrencyValue>, INumericData
    {
        private readonly Lazy<string> _displayText;

        public bool IsNegative => Value.Value < 0;

        public double NumericValueInCurrentUnit => Value.Value;

        public double NumericValueInStandardUnit { get; }

        public override string GetDisplayText(string culture)
        {
            return _displayText.Value; // TODO: Localize.
        }

        public static CurrencyData CreateFrom(CurrencyData origin, CurrencyValue value)
        {
            return new CurrencyData(
                origin.LineTextIncludingLineBreak,
                origin.StartInLine,
                origin.EndInLine,
                value);
        }

        public CurrencyData(string lineTextIncludingLineBreak, int startInLine, int endInLine, CurrencyValue value)
            : base(
                  lineTextIncludingLineBreak,
                  startInLine,
                  endInLine,
                  value,
                  PredefinedTokenAndDataTypeNames.Numeric,
                  PredefinedTokenAndDataTypeNames.SubDataTypeNames.Currency)
        {
            NumericValueInStandardUnit = value.Value; // TODO: Convert to USD.

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

        public INumericData CreateFromStandardUnit(double value)
        {
            // TODO: is this correct currency text and ISO? Should it be localized?
            return CreateFrom(this, new CurrencyValue(value, currency: "Dollars", isoCurrency: "USD"));
        }

        public INumericData CreateFromCurrentUnit(double value)
        {
            return CreateFrom(this, new CurrencyValue(value, Value.Currency, Value.IsoCurrency));
        }

        public INumericData Add(INumericData otherData)
        {
            throw new NotImplementedException();
        }

        public INumericData Substract(INumericData otherData)
        {
            throw new NotImplementedException();
        }

        public INumericData Multiply(INumericData otherData)
        {
            throw new NotImplementedException();
        }

        public INumericData Divide(INumericData otherData)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
