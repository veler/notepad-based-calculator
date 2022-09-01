using System.Globalization;

namespace NotepadBasedCalculator.Api
{
    public sealed record BooleanData : Data<bool>, INumericData
    {
        public bool IsNegative => false;

        public double NumericValueInCurrentUnit => Convert.ToInt32(Value);

        public double NumericValueInStandardUnit => NumericValueInCurrentUnit;

        public override string GetDisplayText(string culture)
        {
            return Value.ToString(new CultureInfo(culture));
            // TODO => Localize. For example, in french, double separator is `,` instead of `.`
        }

        public static BooleanData CreateFrom(BooleanData origin, bool value)
        {
            return new BooleanData(
                origin.LineTextIncludingLineBreak,
                origin.StartInLine,
                origin.EndInLine,
                value);
        }

        public BooleanData(string lineTextIncludingLineBreak, int startInLine, int endInLine, bool value)
            : base(
                  lineTextIncludingLineBreak,
                  startInLine,
                  endInLine,
                  value,
                  PredefinedTokenAndDataTypeNames.Numeric,
                  PredefinedTokenAndDataTypeNames.SubDataTypeNames.Decimal)
        {
        }

        public override IData MergeDataLocations(IData otherData)
        {
            return new BooleanData(
                LineTextIncludingLineBreak,
                Math.Min(StartInLine, otherData.StartInLine),
                Math.Max(EndInLine, otherData.EndInLine),
                Value);
        }

        public INumericData CreateFromStandardUnit(double value)
        {
            if (value == 0 || value == 1)
            {
                return CreateFrom(this, value == 0 ? false : true);
            }

            ThrowHelper.ThrowArgumentException(nameof(value), "The value cannot be converted to a boolean.");
            return null;
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
