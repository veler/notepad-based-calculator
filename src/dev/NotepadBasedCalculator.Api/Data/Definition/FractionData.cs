using System.Globalization;

namespace NotepadBasedCalculator.Api
{
    public sealed record FractionData : Data<double>, INumericData
    {
        public bool IsNegative => Value < 0;

        public double NumericValueInCurrentUnit => Value;

        public double NumericValueInStandardUnit => NumericValueInCurrentUnit;

        public override string GetDisplayText(string culture)
        {
            // TODO: Localize
            return Value.ToString(new CultureInfo(culture));
        }

        public static FractionData CreateFrom(FractionData origin, double value)
        {
            return new FractionData(
                origin.LineTextIncludingLineBreak,
                origin.StartInLine,
                origin.EndInLine,
                value);
        }

        public FractionData(string lineTextIncludingLineBreak, int startInLine, int endInLine, double value)
            : base(
                  lineTextIncludingLineBreak,
                  startInLine,
                  endInLine,
                  value,
                  PredefinedTokenAndDataTypeNames.Numeric,
                  PredefinedTokenAndDataTypeNames.SubDataTypeNames.Fraction)
        {
        }

        public override IData MergeDataLocations(IData otherData)
        {
            return new FractionData(
                LineTextIncludingLineBreak,
                Math.Min(StartInLine, otherData.StartInLine),
                Math.Max(EndInLine, otherData.EndInLine),
                Value);
        }

        public INumericData CreateFromStandardUnit(double value)
        {
            return CreateFrom(this, value);
        }

        public INumericData CreateFromCurrentUnit(double value)
        {
            return CreateFrom(this, value);
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
