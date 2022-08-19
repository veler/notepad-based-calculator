namespace NotepadBasedCalculator.Api
{
    public struct UnitFloat
    {
        public UnitFloat(string unit, float value)
        {
            Unit = unit;
            Value = value;
        }

        public string Unit { get; }

        public float Value { get; }
    }
}
