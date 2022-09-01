using System.Globalization;
using UnitsNet;
using UnitsNet.Units;

namespace NotepadBasedCalculator.Api
{
    public sealed record AreaData : Data<Area>, INumericData
    {
        public bool IsNegative => Value.Value < 0;

        public double NumericValueInCurrentUnit => (double)Value.Value;

        public double NumericValueInStandardUnit { get; }

        public override string GetDisplayText(string culture)
        {
            return ToBestUnitForDisplay(Value).ToString("s4", new CultureInfo(culture));
        }

        public static AreaData CreateFrom(AreaData origin, Area value)
        {
            return new AreaData(
                origin.LineTextIncludingLineBreak,
                origin.StartInLine,
                origin.EndInLine,
                value);
        }

        public AreaData(string lineTextIncludingLineBreak, int startInLine, int endInLine, Area value)
            : base(
                  lineTextIncludingLineBreak,
                  startInLine,
                  endInLine,
                  value,
                  PredefinedTokenAndDataTypeNames.Numeric,
                  PredefinedTokenAndDataTypeNames.SubDataTypeNames.Area)
        {
            NumericValueInStandardUnit = value.ToUnit(Area.BaseUnit).Value;
        }

        public override IData MergeDataLocations(IData otherData)
        {
            return new AreaData(
                LineTextIncludingLineBreak,
                Math.Min(StartInLine, otherData.StartInLine),
                Math.Max(EndInLine, otherData.EndInLine),
                Value);
        }

        public INumericData CreateFromStandardUnit(double value)
        {
            return CreateFrom(this, new Area(value, Area.BaseUnit));
        }

        public INumericData CreateFromCurrentUnit(double value)
        {
            return CreateFrom(this, new Area(value, Value.Unit));
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

        private static Area ToBestUnitForDisplay(Area area)
        {
            if (area.Unit == AreaUnit.SquareMeter && area.SquareMeters >= 1_000)
            {
                return area.ToUnit(AreaUnit.SquareKilometer);
            }

            if (area.Unit == AreaUnit.SquareKilometer && area.SquareMeters < 1_000)
            {
                return area.ToUnit(AreaUnit.SquareKilometer);
            }

            return area;
        }
    }
}
