namespace NotepadBasedCalculator.Api
{
    public abstract record Data<T> : Token, IData<T>
    {
        public string? Subtype { get; }

        public T Value { get; }

        protected Data(
            string lineTextIncludingLineBreak,
            int startInLine,
            int endInLine,
            T value,
            string dataType,
            string? subtype = null)
            : base(lineTextIncludingLineBreak, startInLine, endInLine, dataType)
        {
            Subtype = subtype;
            Value = value;
        }

        public bool Equals(IData? other)
        {
            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            if (other is not null
                && StartInLine == other.StartInLine
                && Length == other.Length)
            {
                return true;
            }

            return false;
        }

        public int CompareTo(IData? other)
        {
            return StartInLine.CompareTo(other?.StartInLine ?? 0);
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
