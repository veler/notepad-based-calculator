using UnitsNet;
using UnitsNet.Units;

namespace NotepadBasedCalculator.Api
{
    public sealed record LengthData : Data<Length>, IConvertibleNumericData
    {
        public bool IsNegative => Value.Value < 0;

        public double NumericValueInCurrentUnit => (double)Value.Value;

        public override string DisplayText => $"{Value}"; // TODO => Localize

        public LengthData(string lineTextIncludingLineBreak, int startInLine, int endInLine, Length value)
            : base(
                  lineTextIncludingLineBreak,
                  startInLine,
                  endInLine,
                  value,
                  PredefinedTokenAndDataTypeNames.Numeric,
                  PredefinedTokenAndDataTypeNames.SubDataTypeNames.Length)
        {
        }

        public override IData MergeDataLocations(IData otherData)
        {
            return new LengthData(
                LineTextIncludingLineBreak,
                Math.Min(StartInLine, otherData.StartInLine),
                Math.Max(EndInLine, otherData.EndInLine),
                Value);
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
