using UnitsNet;
using UnitsNet.Units;

namespace NotepadBasedCalculator.Api
{
    public sealed record InformationData : Data<Information>, IConvertibleNumericData
    {
        public bool IsNegative => Value.Value < 0;

        public double NumericValue => (double)Value.Value;

        public override string DisplayText => $"{Value}"; // TODO => Localize

        public InformationData(string lineTextIncludingLineBreak, int startInLine, int endInLine, Information value)
            : base(
                  lineTextIncludingLineBreak,
                  startInLine,
                  endInLine,
                  value,
                  PredefinedTokenAndDataTypeNames.Numeric,
                  PredefinedTokenAndDataTypeNames.SubDataTypeNames.Information)
        {
        }

        public override IData MergeDataLocations(IData otherData)
        {
            return new InformationData(
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
            return new InformationData(
                LineTextIncludingLineBreak,
                StartInLine,
                EndInLine,
                Value.ToUnit(InformationUnit.Byte));
        }

        public INumericData FromStandardUnit(double newStandardUnitValue)
        {
            return new InformationData(
                LineTextIncludingLineBreak,
                StartInLine,
                EndInLine,
                Information.FromBytes(newStandardUnitValue).ToUnit(Value.Unit));
        }

        public INumericData? ConvertFrom(INumericData from)
        {
            if (from is InformationData fromSameData)
            {
                return new InformationData(
                    from.LineTextIncludingLineBreak,
                    from.StartInLine,
                    from.EndInLine,
                    fromSameData.Value.ToUnit(Value.Unit));
            }

            return new InformationData(
                from.LineTextIncludingLineBreak,
                from.StartInLine,
                from.EndInLine,
                new Information((decimal)from.NumericValue, Value.Unit));
        }

        public bool CanConvertFrom(INumericData from)
        {
            return from is DecimalData or InformationData;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
