using UnitsNet;
using UnitsNet.Units;

namespace NotepadBasedCalculator.BuiltInPlugins.Data.Definition
{
    public sealed record LengthData : Data<Length>, IConvertibleNumericData
    {
        public bool IsNegative => Value.Value < 0;

        public double NumericValue => (double)Value.Value;

        public override string DisplayText => $"{Value}"; // TODO => Localize

        public LengthData(string lineTextIncludingLineBreak, int startInLine, int endInLine, Length value)
            : base(
                  lineTextIncludingLineBreak,
                  startInLine,
                  endInLine,
                  value,
                  PredefinedTokenAndDataTypeNames.Numeric,
                  PredefinedTokenAndDataTypeNames.SubDataTypeNames.Length)
        {
        }

        public override IData MergeDataLocations(IData otherData)
        {
            return new LengthData(
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
            return new LengthData(
                LineTextIncludingLineBreak,
                StartInLine,
                EndInLine,
                Value.ToUnit(LengthUnit.Meter));
        }

        public INumericData FromStandardUnit(double newStandardUnitValue)
        {
            return new LengthData(
                LineTextIncludingLineBreak,
                StartInLine,
                EndInLine,
                UnitsNet.Length.FromMeters(newStandardUnitValue).ToUnit(Value.Unit));
        }

        public INumericData? ConvertFrom(INumericData from)
        {
            return new LengthData(
                from.LineTextIncludingLineBreak,
                from.StartInLine,
                from.EndInLine,
                new Length(from.NumericValue, Value.Unit));
        }

        public bool CanConvertFrom(INumericData from)
        {
            return from is DecimalData or LengthData;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
