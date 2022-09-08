using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.NumberWithUnit;

namespace NotepadBasedCalculator.BuiltInPlugins.Data
{
    [Export(typeof(IDataParser))]
    [Culture(SupportedCultures.Any)]
    public sealed class CurrencyDataParser : IDataParser
    {
        private const string Value = "value";
        private const string Unit = "unit";
        private const string IsoCurrency = "isoCurrency";
        private const string TypeName = "currency";

        private readonly ICurrencyService _currencyService;

        [ImportingConstructor]
        public CurrencyDataParser(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        public IReadOnlyList<IData>? Parse(string culture, TokenizedTextLine tokenizedTextLine, CancellationToken cancellationToken)
        {
            List<ModelResult> modelResults = NumberWithUnitRecognizer.RecognizeCurrency(tokenizedTextLine.LineTextIncludingLineBreak, culture);
            cancellationToken.ThrowIfCancellationRequested();

            var data = new List<IData>();

            for (int i = 0; i < modelResults.Count; i++)
            {
                ModelResult modelResult = modelResults[i];
                if (modelResult.Resolution is not null)
                {
                    switch (modelResult.TypeName)
                    {
                        case TypeName:
                            string valueString = (string)modelResult.Resolution[Value];
                            string unit = (string)modelResult.Resolution[Unit];
                            string isoCurrency = string.Empty;
                            if (modelResult.Resolution.TryGetValue(IsoCurrency, out object? isoCurrencyObject))
                            {
                                isoCurrency = isoCurrencyObject as string ?? string.Empty;
                            }

                            data.Add(
                                new CurrencyData(
                                    _currencyService,
                                    tokenizedTextLine.LineTextIncludingLineBreak,
                                    modelResult.Start,
                                    modelResult.End + 1,
                                    new CurrencyValue(
                                        double.Parse(valueString),
                                        unit,
                                        isoCurrency)));

                            break;

                        default:
#if DEBUG
                            ThrowHelper.ThrowNotSupportedException();
#endif
                            break;
                    }
                }
            }

            return data;
        }
    }
}
