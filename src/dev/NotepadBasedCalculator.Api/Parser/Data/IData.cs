namespace NotepadBasedCalculator.Api
{
    public interface IData : IEquatable<IData>, IComparable<IData>
    {
        /// <summary>
        /// Gets the position in the line where the data starts.
        /// </summary>
        int StartInLine { get; }

        /// <summary>
        /// Gets the position in the line where the data ends.
        /// </summary>
        int EndInLine { get; }

        /// <summary>
        /// Gets the length of the data.
        /// </summary>
        int Length { get; }

        string OriginalText { get; }
    }
}
