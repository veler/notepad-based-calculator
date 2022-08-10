using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.Number;
using Constants = Microsoft.Recognizers.Text.Number.Constants;

namespace NotepadBasedCalculator.BuiltInPlugins.Data
{
    [Export(typeof(IDataParser))]
    [Culture(SupportedCultures.Any)]
    public sealed class PercentageDataParser : IDataParser
    {
        private const string Value = "value";

        public IReadOnlyList<IData>? Parse(string culture, TokenizedTextLine tokenizedTextLine)
        {
            List<ModelResult> modelResults = NumberRecognizer.RecognizePercentage(tokenizedTextLine.LineTextIncludingLineBreak, culture);
            var data = new List<IData>();

            for (int i = 0; i < modelResults.Count; i++)
            {
                ModelResult modelResult = modelResults[i];
                switch (modelResult.TypeName)
                {
                    case Constants.MODEL_PERCENTAGE:
                        string valueString = (string)modelResult.Resolution[Value];
                        valueString = valueString.TrimEnd('%');
                        data.Add(new PercentageData(modelResult.Start, modelResult.Text, float.Parse(valueString)));
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
