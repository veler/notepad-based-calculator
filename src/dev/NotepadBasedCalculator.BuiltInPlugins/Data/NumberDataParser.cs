using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.Number;
using Constants = Microsoft.Recognizers.Text.Number.Constants;

namespace NotepadBasedCalculator.BuiltInPlugins.Data
{
    [Export(typeof(IDataParser))]
    [Culture(SupportedCultures.Any)]
    public sealed class NumberDataParser : IDataParser
    {
        private const string SubType = "subtype";
        private const string Value = "value";

        public IReadOnlyList<IData>? Parse(string culture, TokenizedTextLine tokenizedTextLine)
        {
            List<ModelResult> modelResults = NumberRecognizer.RecognizeNumber(tokenizedTextLine.LineTextIncludingLineBreak, culture);
            var data = new List<IData>();

            for (int i = 0; i < modelResults.Count; i++)
            {
                ModelResult modelResult = modelResults[i];
                string valueString = (string)modelResult.Resolution[Value];
                switch (modelResult.Resolution[SubType])
                {
                    case Constants.INTEGER:
                        data.Add(new IntegerData(modelResult.Start, modelResult.Text, int.Parse(valueString)));
                        break;

                    case Constants.DECIMAL:
                    case Constants.POWER:
                        data.Add(new DecimalData(modelResult.Start, modelResult.Text, float.Parse(valueString)));
                        break;

                    case Constants.FRACTION:
                        data.Add(new FractionData(modelResult.Start, modelResult.Text, float.Parse(valueString)));
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
