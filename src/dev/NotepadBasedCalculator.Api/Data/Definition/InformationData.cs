using System.Globalization;
using UnitsNet;

namespace NotepadBasedCalculator.Api
{
    public sealed record InformationData : Data<Information>, INumericData
    {
        public bool IsNegative => Value.Value < 0;

        public double NumericValueInCurrentUnit => (double)Value.Value;

        public double NumericValueInStandardUnit { get; }

        public override string GetDisplayText(string culture)
        {
            return Value.ToString("s4", new CultureInfo(culture));
        }

        public static InformationData CreateFrom(InformationData origin, Information value)
        {
            return new InformationData(
                origin.LineTextIncludingLineBreak,
                origin.StartInLine,
                origin.EndInLine,
                value);
        }

        public InformationData(string lineTextIncludingLineBreak, int startInLine, int endInLine, Information value)
            : base(
                  lineTextIncludingLineBreak,
                  startInLine,
                  endInLine,
                  value,
                  PredefinedTokenAndDataTypeNames.Numeric,
                  PredefinedTokenAndDataTypeNames.SubDataTypeNames.Information)
        {
            NumericValueInStandardUnit = (double)value.ToUnit(Information.BaseUnit).Value;
        }

        public override IData MergeDataLocations(IData otherData)
        {
            return new InformationData(
                LineTextIncludingLineBreak,
                Math.Min(StartInLine, otherData.StartInLine),
                Math.Max(EndInLine, otherData.EndInLine),
                Value);
        }

        public INumericData CreateFromStandardUnit(double value)
        {
            return CreateFrom(this, new Information((decimal)value, Information.BaseUnit));
        }

        public INumericData CreateFromCurrentUnit(double value)
        {
            return CreateFrom(this, new Information((decimal)value, Value.Unit));
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
