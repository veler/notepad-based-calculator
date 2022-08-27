using UnitsNet;
using UnitsNet.Units;

namespace NotepadBasedCalculator.Api
{
    public sealed record TemperatureData : Data<Temperature>, IConvertibleNumericData
    {
        public bool IsNegative => Value.Value < 0;

        public double NumericValue => (double)Value.Value;

        public override string DisplayText => $"{Value}"; // TODO => Localize

        public TemperatureData(string lineTextIncludingLineBreak, int startInLine, int endInLine, Temperature value)
            : base(
                  lineTextIncludingLineBreak,
                  startInLine,
                  endInLine,
                  value,
                  PredefinedTokenAndDataTypeNames.Numeric,
                  PredefinedTokenAndDataTypeNames.SubDataTypeNames.Temperature)
        {
        }

        public override IData MergeDataLocations(IData otherData)
        {
            return new TemperatureData(
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
            return new TemperatureData(
                LineTextIncludingLineBreak,
                StartInLine,
                EndInLine,
                Value.ToUnit(TemperatureUnit.Kelvin));
        }

        public INumericData FromStandardUnit(double newStandardUnitValue)
        {
            return new TemperatureData(
                LineTextIncludingLineBreak,
                StartInLine,
                EndInLine,
                Temperature.FromKelvins(newStandardUnitValue).ToUnit(Value.Unit));
        }

        public INumericData? ConvertFrom(INumericData from)
        {
            if (from is TemperatureData fromSameData)
            {
                return new TemperatureData(
                    from.LineTextIncludingLineBreak,
                    from.StartInLine,
                    from.EndInLine,
                    fromSameData.Value.ToUnit(Value.Unit));
            }

            return new TemperatureData(
                from.LineTextIncludingLineBreak,
                from.StartInLine,
                from.EndInLine,
                new Temperature(from.NumericValue, Value.Unit));
        }

        public bool CanConvertFrom(INumericData from)
        {
            return from is DecimalData or TemperatureData;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
