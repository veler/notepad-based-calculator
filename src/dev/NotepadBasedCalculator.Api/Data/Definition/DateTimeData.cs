using UnitsNet.Units;
using UnitsNet;
using System.Globalization;

namespace NotepadBasedCalculator.Api
{
    public sealed record DateTimeData : Data<DateTime>, INumericData
    {
        public bool IsNegative => Value.Ticks < 0;

        public double NumericValueInCurrentUnit => Value.Ticks;

        public double NumericValueInStandardUnit => NumericValueInCurrentUnit;

        public override string GetDisplayText(string culture)
        {
            return Value.ToString(new CultureInfo(culture));
        }

        public static DateTimeData CreateFrom(DateTimeData origin, DateTime value)
        {
            return new DateTimeData(
                origin.LineTextIncludingLineBreak,
                origin.StartInLine,
                origin.EndInLine,
                value);
        }

        public DateTimeData(string lineTextIncludingLineBreak, int startInLine, int endInLine, DateTime value)
            : base(
                  lineTextIncludingLineBreak,
                  startInLine,
                  endInLine,
                  value,
                  PredefinedTokenAndDataTypeNames.Numeric,
                  PredefinedTokenAndDataTypeNames.SubDataTypeNames.DateTime)
        {
        }

        public override IData MergeDataLocations(IData otherData)
        {
            return new DateTimeData(
                LineTextIncludingLineBreak,
                Math.Min(StartInLine, otherData.StartInLine),
                Math.Max(EndInLine, otherData.EndInLine),
                Value);
        }

        public INumericData CreateFromStandardUnit(double value)
        {
            return CreateFrom(this, new DateTime((long)value));
        }

        public INumericData CreateFromCurrentUnit(double value)
        {
            return CreateFromStandardUnit(value);
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
