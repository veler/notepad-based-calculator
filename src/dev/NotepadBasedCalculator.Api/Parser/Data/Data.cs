namespace NotepadBasedCalculator.Api
{
    public abstract record Data<T> : IData
    {
        /// <summary>
        /// Gets the position in the line where the data starts.
        /// </summary>
        public int StartInLine { get; }

        /// <summary>
        /// Gets the position in the line where the data ends.
        /// </summary>
        public int EndInLine { get; }

        /// <summary>
        /// Gets the length of the data.
        /// </summary>
        public int Length { get; }

        public string OriginalText { get; }

        public T Value { get; }

        public Data(int startInLine, string originalText, T value)
        {
            Guard.IsNotNull(originalText);
            Value = value;
            OriginalText = originalText;
            StartInLine = startInLine;
            Length = OriginalText.Length;
            EndInLine = StartInLine + Length;
        }

        public bool Equals(IData other)
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

        public int CompareTo(IData other)
        {
            return StartInLine.CompareTo(other.StartInLine);
        }
    }
}
