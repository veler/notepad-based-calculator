using UnitsNet;
using UnitsNet.Units;

namespace NotepadBasedCalculator.Api
{
    public sealed record AngleData : Data<Angle>, IConvertibleNumericData
    {
        public bool IsNegative => Value.Value < 0;

        public double NumericValue => (double)Value.Value;

        public override string DisplayText => $"{Value}"; // TODO => Localize

        public AngleData(string lineTextIncludingLineBreak, int startInLine, int endInLine, Angle value)
            : base(
                  lineTextIncludingLineBreak,
                  startInLine,
                  endInLine,
                  value,
                  PredefinedTokenAndDataTypeNames.Numeric,
                  PredefinedTokenAndDataTypeNames.SubDataTypeNames.Angle)
        {
        }

        public override IData MergeDataLocations(IData otherData)
        {
            return new AngleData(
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
            return new AngleData(
                LineTextIncludingLineBreak,
                StartInLine,
                EndInLine,
                Value.ToUnit(AngleUnit.Degree));
        }

        public INumericData FromStandardUnit(double newStandardUnitValue)
        {
            return new AngleData(
                LineTextIncludingLineBreak,
                StartInLine,
                EndInLine,
                Angle.FromDegrees(newStandardUnitValue).ToUnit(Value.Unit));
        }

        public INumericData? ConvertFrom(INumericData from)
        {
            if (from is AngleData fromAngleData)
            {
                return new AngleData(
                    from.LineTextIncludingLineBreak,
                    from.StartInLine,
                    from.EndInLine,
                    fromAngleData.Value.ToUnit(Value.Unit));
            }

            return new AngleData(
                from.LineTextIncludingLineBreak,
                from.StartInLine,
                from.EndInLine,
                new Angle(from.NumericValue, Value.Unit));
        }

        public bool CanConvertFrom(INumericData from)
        {
            return from is DecimalData or AngleData;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
