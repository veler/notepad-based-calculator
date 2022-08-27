using UnitsNet;
using UnitsNet.Units;

namespace NotepadBasedCalculator.BuiltInPlugins.Data.Definition
{
    public sealed record MassData : Data<Mass>, IConvertibleNumericData
    {
        public bool IsNegative => Value.Value < 0;

        public double NumericValue => (double)Value.Value;

        public override string DisplayText => $"{Value}"; // TODO => Localize

        public MassData(string lineTextIncludingLineBreak, int startInLine, int endInLine, Mass value)
            : base(
                  lineTextIncludingLineBreak,
                  startInLine,
                  endInLine,
                  value,
                  PredefinedTokenAndDataTypeNames.Numeric,
                  PredefinedTokenAndDataTypeNames.SubDataTypeNames.Mass)
        {
        }

        public override IData MergeDataLocations(IData otherData)
        {
            return new MassData(
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
            return new MassData(
                LineTextIncludingLineBreak,
                StartInLine,
                EndInLine,
                Value.ToUnit(MassUnit.Gram));
        }

        public INumericData FromStandardUnit(double newStandardUnitValue)
        {
            return new MassData(
                LineTextIncludingLineBreak,
                StartInLine,
                EndInLine,
                Mass.FromGrams(newStandardUnitValue).ToUnit(Value.Unit));
        }

        public INumericData? ConvertFrom(INumericData from)
        {
            return new MassData(
                from.LineTextIncludingLineBreak,
                from.StartInLine,
                from.EndInLine,
                new Mass(from.NumericValue, Value.Unit));
        }

        public bool CanConvertFrom(INumericData from)
        {
            return from is DecimalData or MassData;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
