using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.Number;
using Constants = Microsoft.Recognizers.Text.Number.Constants;

namespace NotepadBasedCalculator.BuiltInPlugins.Data
{
    [Export(typeof(IDataParser))]
    [Culture(SupportedCultures.Any)]
    public sealed class OrdinalDataParser : IDataParser
    {
        private const string Value = "value";

        public IReadOnlyList<IData>? Parse(string culture, TokenizedTextLine tokenizedTextLine)
        {
            List<ModelResult> modelResults = NumberRecognizer.RecognizeOrdinal(tokenizedTextLine.LineTextIncludingLineBreak, culture);
            var data = new List<IData>();

            for (int i = 0; i < modelResults.Count; i++)
            {
                ModelResult modelResult = modelResults[i];
                string valueString = (string)modelResult.Resolution[Value];
                switch (modelResult.TypeName)
                {
                    case Constants.MODEL_ORDINAL:
                        data.Add(new OrdinalData(modelResult.Start, modelResult.Text, int.Parse(valueString)));
                        break;

                    case Constants.MODEL_ORDINAL_RELATIVE:
                        // TODO
                        ThrowHelper.ThrowNotSupportedException();
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
