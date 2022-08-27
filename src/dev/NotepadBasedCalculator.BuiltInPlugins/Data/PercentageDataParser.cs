using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.Number;
using Constants = Microsoft.Recognizers.Text.Number.Constants;

namespace NotepadBasedCalculator.BuiltInPlugins.Data
{
    [Export(typeof(IDataParser))]
    [Culture(SupportedCultures.Any)]
    [Shared]
    public sealed class PercentageDataParser : IDataParser
    {
        private const string Value = "value";
        private const string NullValue = "null";

        public IReadOnlyList<IData>? Parse(string culture, TokenizedTextLine tokenizedTextLine, CancellationToken cancellationToken)
        {
            List<ModelResult> modelResults = NumberRecognizer.RecognizePercentage(tokenizedTextLine.LineTextIncludingLineBreak, culture);
            cancellationToken.ThrowIfCancellationRequested();

            var data = new List<IData>();

            for (int i = 0; i < modelResults.Count; i++)
            {
                ModelResult modelResult = modelResults[i];
                switch (modelResult.TypeName)
                {
                    case Constants.MODEL_PERCENTAGE:
                        string valueString = (string)modelResult.Resolution[Value];
                        if (!string.Equals(NullValue, valueString, StringComparison.OrdinalIgnoreCase))
                        {
                            valueString = valueString.TrimEnd('%');
                            data.Add(
                                new PercentageData(
                                    tokenizedTextLine.LineTextIncludingLineBreak,
                                    modelResult.Start,
                                    modelResult.End + 1,
                                    double.Parse(valueString) / 100));
                        }
                        break;

                    default:
#if DEBUG
                        ThrowHelper.ThrowNotSupportedException();
#endif
                        break;
                }
            }

            return data;
        }
    }
}
