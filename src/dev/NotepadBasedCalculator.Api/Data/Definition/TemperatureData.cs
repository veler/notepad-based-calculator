using System.Globalization;
using UnitsNet;
using UnitsNet.Units;

namespace NotepadBasedCalculator.Api
{
    public sealed record TemperatureData : Data<Temperature>, INumericData
    {
        public bool IsNegative => Value.Value < 0;

        public double NumericValueInCurrentUnit => (double)Value.Value;

        public double NumericValueInStandardUnit { get; }

        public override string GetDisplayText(string culture)
        {
            return Value.ToString("s4", new CultureInfo(culture));
        }

        public static TemperatureData CreateFrom(TemperatureData origin, Temperature value)
        {
            return new TemperatureData(
                origin.LineTextIncludingLineBreak,
                origin.StartInLine,
                origin.EndInLine,
                value);
        }

        public TemperatureData(string lineTextIncludingLineBreak, int startInLine, int endInLine, Temperature value)
            : base(
                  lineTextIncludingLineBreak,
                  startInLine,
                  endInLine,
                  value,
                  PredefinedTokenAndDataTypeNames.Numeric,
                  PredefinedTokenAndDataTypeNames.SubDataTypeNames.Temperature)
        {
            NumericValueInStandardUnit = value.ToUnit(Temperature.BaseUnit).Value;
        }

        public override IData MergeDataLocations(IData otherData)
        {
            return new TemperatureData(
                LineTextIncludingLineBreak,
                Math.Min(StartInLine, otherData.StartInLine),
                Math.Max(EndInLine, otherData.EndInLine),
                Value);
        }

        public INumericData CreateFromStandardUnit(double value)
        {
            return CreateFrom(this, new Temperature(value, Temperature.BaseUnit));
        }

        public INumericData CreateFromCurrentUnit(double value)
        {
            return CreateFrom(this, new Temperature(value, Value.Unit));
        }

        public INumericData Add(INumericData otherData)
        {
            throw new NotImplementedException();
        }

        public INumericData Substract(INumericData otherData)
        {
            throw new NotImplementedException();
        }

        public INumericData Multiply(INumericData otherData)
        {
            throw new NotImplementedException();
        }

        public INumericData Divide(INumericData otherData)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
