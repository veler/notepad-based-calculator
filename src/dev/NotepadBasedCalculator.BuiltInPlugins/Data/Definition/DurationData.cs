namespace NotepadBasedCalculator.BuiltInPlugins.Data.Definition
{
    public sealed record DurationData : Data<TimeSpan>, IConvertibleNumericData
    {
        public bool IsNegative => Value.Ticks < 0;

        public double NumericValue => Value.Ticks;

        public override string DisplayText => $"{Value}"; // TODO => Localize

        public DurationData(string lineTextIncludingLineBreak, int startInLine, int endInLine, TimeSpan value)
            : base(
                  lineTextIncludingLineBreak,
                  startInLine,
                  endInLine,
                  value,
                  PredefinedTokenAndDataTypeNames.Numeric,
                  PredefinedTokenAndDataTypeNames.SubDataTypeNames.Duration)
        {
        }

        public override IData MergeDataLocations(IData otherData)
        {
            return new DurationData(
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
            return this;
        }

        public INumericData FromStandardUnit(double newStandardUnitValue)
        {
            return new DurationData(
                LineTextIncludingLineBreak,
                StartInLine,
                EndInLine,
                TimeSpan.FromTicks((long)newStandardUnitValue));
        }

        public INumericData? ConvertFrom(INumericData from)
        {
            if (from is DecimalData)
            {
                return FromStandardUnit(from.NumericValue);
            }
            else if (from is DurationData durationData)
            {
                return durationData;
            }

            ThrowHelper.ThrowNotSupportedException();
            return null;
        }

        public bool CanConvertFrom(INumericData from)
        {
            return from is DecimalData or DurationData;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
