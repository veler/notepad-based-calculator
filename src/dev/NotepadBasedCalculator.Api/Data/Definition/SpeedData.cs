using UnitsNet;
using UnitsNet.Units;

namespace NotepadBasedCalculator.Api
{
    public sealed record SpeedData : Data<Speed>, IConvertibleNumericData
    {
        public bool IsNegative => Value.Value < 0;

        public double NumericValue => (double)Value.Value;

        public override string DisplayText => $"{Value}"; // TODO => Localize

        public SpeedData(string lineTextIncludingLineBreak, int startInLine, int endInLine, Speed value)
            : base(
                  lineTextIncludingLineBreak,
                  startInLine,
                  endInLine,
                  value,
                  PredefinedTokenAndDataTypeNames.Numeric,
                  PredefinedTokenAndDataTypeNames.SubDataTypeNames.Speed)
        {
        }

        public override IData MergeDataLocations(IData otherData)
        {
            return new SpeedData(
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
            return new SpeedData(
                LineTextIncludingLineBreak,
                StartInLine,
                EndInLine,
                Value.ToUnit(SpeedUnit.MeterPerSecond));
        }

        public INumericData FromStandardUnit(double newStandardUnitValue)
        {
            return new SpeedData(
                LineTextIncludingLineBreak,
                StartInLine,
                EndInLine,
                Speed.FromMetersPerSecond(newStandardUnitValue).ToUnit(Value.Unit));
        }

        public INumericData? ConvertFrom(INumericData from)
        {
            return new SpeedData(
                from.LineTextIncludingLineBreak,
                from.StartInLine,
                from.EndInLine,
                new Speed(from.NumericValue, Value.Unit));
        }

        public bool CanConvertFrom(INumericData from)
        {
            return from is DecimalData or SpeedData;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
