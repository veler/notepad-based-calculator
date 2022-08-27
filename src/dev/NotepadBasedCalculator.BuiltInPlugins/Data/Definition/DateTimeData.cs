namespace NotepadBasedCalculator.BuiltInPlugins.Data.Definition
{
    public sealed record DateTimeData : Data<DateTime>, IConvertibleNumericData
    {
        public bool IsNegative => Value.Ticks < 0;

        public double NumericValue => Value.Ticks;

        public override string DisplayText => $"{Value}"; // TODO => Localize

        public DateTimeData(string lineTextIncludingLineBreak, int startInLine, int endInLine, DateTime value)
            : base(
                  lineTextIncludingLineBreak,
                  startInLine,
                  endInLine,
                  value,
                  PredefinedTokenAndDataTypeNames.Numeric,
                  PredefinedTokenAndDataTypeNames.SubDataTypeNames.DateTime)
        {
        }

        public override IData MergeDataLocations(IData otherData)
        {
            return new DateTimeData(
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
            return new DurationData(
                LineTextIncludingLineBreak,
                StartInLine,
                EndInLine,
                TimeSpan.FromTicks(Value.Ticks));
        }

        public INumericData FromStandardUnit(double newStandardUnitValue)
        {
            return new DateTimeData(
                LineTextIncludingLineBreak,
                StartInLine,
                EndInLine,
                new DateTime(ticks: (long)newStandardUnitValue));
        }

        public INumericData? ConvertFrom(INumericData from)
        {
            if (from is DecimalData)
            {
                return FromStandardUnit(from.NumericValue);
            }
            else if (from is DateTimeData dateTimeData)
            {
                return dateTimeData;
            }
            else if (from is DurationData durationData)
            {
                return FromStandardUnit(durationData.Value.Ticks);
            }

            ThrowHelper.ThrowNotSupportedException();
            return null;
        }

        public bool CanConvertFrom(INumericData from)
        {
            return from is DecimalData or DateTimeData or DurationData;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
