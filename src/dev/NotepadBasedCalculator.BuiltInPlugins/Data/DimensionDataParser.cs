using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.NumberWithUnit;
using Constants = Microsoft.Recognizers.Text.NumberWithUnit.Constants;

namespace NotepadBasedCalculator.BuiltInPlugins.Data
{
    [Export(typeof(IDataParser))]
    [Culture(SupportedCultures.Any)]
    public sealed class DimensionDataParser : IDataParser
    {
        private const string Value = "value";
        private const string Unit = "unit";
        private const string Subtype = "subtype";

        public IReadOnlyList<IData>? Parse(string culture, TokenizedTextLine tokenizedTextLine)
        {
            List<ModelResult> modelResults = NumberWithUnitRecognizer.RecognizeDimension(tokenizedTextLine.LineTextIncludingLineBreak, culture);
            var data = new List<IData>();

            for (int i = 0; i < modelResults.Count; i++)
            {
                ModelResult modelResult = modelResults[i];

                string valueString = (string)modelResult.Resolution[Value];
                string unit = (string)modelResult.Resolution[Unit];
                string subType;

                switch (modelResult.Resolution[Subtype])
                {
                    case Constants.LENGTH:
                        subType = PredefinedTokenAndDataTypeNames.SubDataTypeNames.Length;
                        break;

                    case Constants.INFORMATION:
                        subType = PredefinedTokenAndDataTypeNames.SubDataTypeNames.Information;
                        break;

                    case Constants.AREA:
                        subType = PredefinedTokenAndDataTypeNames.SubDataTypeNames.Area;
                        break;

                    case Constants.SPEED:
                        subType = PredefinedTokenAndDataTypeNames.SubDataTypeNames.Speed;
                        break;

                    case Constants.VOLUME:
                        subType = PredefinedTokenAndDataTypeNames.SubDataTypeNames.Volume;
                        break;

                    case Constants.WEIGHT:
                        subType = PredefinedTokenAndDataTypeNames.SubDataTypeNames.Weight;
                        break;

                    case Constants.ANGLE:
                        subType = PredefinedTokenAndDataTypeNames.SubDataTypeNames.Angle;
                        break;

                    default:
                        ThrowHelper.ThrowNotSupportedException();
                        return null;
                }

                data.Add(
                    new UnitData(
                        tokenizedTextLine.LineTextIncludingLineBreak,
                        modelResult.Start,
                        modelResult.End + 1,
                        subType,
                        new UnitFloat(unit, float.Parse(valueString))));
            }

            return data;
        }
    }
}
