using UnitsNet;

namespace NotepadBasedCalculator.Api
{
    public sealed record AngleData : Data<Angle>, IConvertibleNumericData
    {
        public bool IsNegative => Value.Value < 0;

        public double NumericValueInCurrentUnit => (double)Value.Value;

        public double NumericValueInStandardUnit { get; }

        public override string DisplayText => $"{Value}"; // TODO => Localize

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
            NumericValueInStandardUnit = value.ToUnit(UnitsNet.Units.AngleUnit.Radian).Value;
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
            return CreateFrom(this, new Angle(value, UnitsNet.Units.AngleUnit.Radian));
        }

        public INumericData CreateFromCurrentUnit(double value)
        {
            return CreateFrom(this, new Angle(value, Value.Unit));
        }

        public IConvertibleNumericData ConvertTo(INumericData data)
        {
            Guard.IsOfType<AngleData>(data);
            var angle = (AngleData)data;
            return CreateFrom(this, Value.ToUnit(angle.Value.Unit));
        }

        public bool CanConvertTo(INumericData data)
        {
            return data is AngleData;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
