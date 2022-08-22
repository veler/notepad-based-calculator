using UnitsNet;
using UnitsNet.Units;

namespace NotepadBasedCalculator.BuiltInPlugins.Data.Definition
{
    public sealed record AreaData : Data<Area>, IConvertibleNumericData
    {
        public bool IsNegative => Value.Value < 0;

        public double NumericValue => (double)Value.Value;

        public override string DisplayText => $"{Value.ToString()}"; // TODO => Localize

        public AreaData(string lineTextIncludingLineBreak, int startInLine, int endInLine, Area value)
            : base(
                  lineTextIncludingLineBreak,
                  startInLine,
                  endInLine,
                  value,
                  PredefinedTokenAndDataTypeNames.Numeric,
                  PredefinedTokenAndDataTypeNames.SubDataTypeNames.Area)
        {
        }

        public override IData MergeDataLocations(IData otherData)
        {
            return new AreaData(
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
            return new AreaData(
                LineTextIncludingLineBreak,
                StartInLine,
                EndInLine,
                Value.ToUnit(AreaUnit.SquareMeter));
        }

        public INumericData FromStandardUnit(double newStandardUnitValue)
        {
            return new AreaData(
                LineTextIncludingLineBreak,
                StartInLine,
                EndInLine,
                Area.FromSquareMeters(newStandardUnitValue).ToUnit(Value.Unit));
        }

        public INumericData? ConvertFrom(INumericData from)
        {
            return new AreaData(
                from.LineTextIncludingLineBreak,
                from.StartInLine,
                from.EndInLine,
                new Area(from.NumericValue, Value.Unit));
        }

        public bool CanConvertFrom(INumericData from)
        {
            return from is DecimalData or AreaData;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
