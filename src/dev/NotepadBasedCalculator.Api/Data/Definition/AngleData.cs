using System.Globalization;
using UnitsNet;

namespace NotepadBasedCalculator.Api
{
    public sealed record AngleData : Data<Angle>, INumericData
    {
        public bool IsNegative => Value.Value < 0;

        public double NumericValueInCurrentUnit => (double)Value.Value;

        public double NumericValueInStandardUnit { get; }

        public override string GetDisplayText(string culture)
        {
            return Value.ToString("s4", new CultureInfo(culture));
        }

        public static AngleData CreateFrom(AngleData origin, Angle value)
        {
            return new AngleData(
                origin.LineTextIncludingLineBreak,
                origin.StartInLine,
                origin.EndInLine,
                value);
        }

        public AngleData(string lineTextIncludingLineBreak, int startInLine, int endInLine, Angle value)
            : base(
                  lineTextIncludingLineBreak,
                  startInLine,
                  endInLine,
                  value,
                  PredefinedTokenAndDataTypeNames.Numeric,
                  PredefinedTokenAndDataTypeNames.SubDataTypeNames.Angle)
        {
            NumericValueInStandardUnit = value.ToUnit(Angle.BaseUnit).Value;
        }

        public override IData MergeDataLocations(IData otherData)
        {
            return new AngleData(
                LineTextIncludingLineBreak,
                Math.Min(StartInLine, otherData.StartInLine),
                Math.Max(EndInLine, otherData.EndInLine),
                Value);
        }

        public INumericData CreateFromStandardUnit(double value)
        {
            return CreateFrom(this, new Angle(value, Angle.BaseUnit));
        }

        public INumericData CreateFromCurrentUnit(double value)
        {
            return CreateFrom(this, new Angle(value, Value.Unit));
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
