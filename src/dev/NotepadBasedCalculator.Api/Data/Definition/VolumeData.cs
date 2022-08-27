using UnitsNet;
using UnitsNet.Units;

namespace NotepadBasedCalculator.Api
{
    public sealed record VolumeData : Data<Volume>, IConvertibleNumericData
    {
        public bool IsNegative => Value.Value < 0;

        public double NumericValue => (double)Value.Value;

        public override string DisplayText => $"{Value}"; // TODO => Localize

        public VolumeData(string lineTextIncludingLineBreak, int startInLine, int endInLine, Volume value)
            : base(
                  lineTextIncludingLineBreak,
                  startInLine,
                  endInLine,
                  value,
                  PredefinedTokenAndDataTypeNames.Numeric,
                  PredefinedTokenAndDataTypeNames.SubDataTypeNames.Volume)
        {
        }

        public override IData MergeDataLocations(IData otherData)
        {
            return new VolumeData(
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
            return new VolumeData(
                LineTextIncludingLineBreak,
                StartInLine,
                EndInLine,
                Value.ToUnit(VolumeUnit.Liter));
        }

        public INumericData FromStandardUnit(double newStandardUnitValue)
        {
            return new VolumeData(
                LineTextIncludingLineBreak,
                StartInLine,
                EndInLine,
                Volume.FromLiters(newStandardUnitValue).ToUnit(Value.Unit));
        }

        public INumericData? ConvertFrom(INumericData from)
        {
            return new VolumeData(
                from.LineTextIncludingLineBreak,
                from.StartInLine,
                from.EndInLine,
                new Volume(from.NumericValue, Value.Unit));
        }

        public bool CanConvertFrom(INumericData from)
        {
            return from is DecimalData or VolumeData;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
