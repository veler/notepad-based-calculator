namespace NotepadBasedCalculator.Api
{
    public interface IData : IToken, IEquatable<IData>, IComparable<IData>
    {
        /// <summary>
        /// When multiple data or various type are discovered the text document at a same location, the parser
        /// needs to solve the conflict. This property indicates the priority to of a data over others.
        /// <see cref="int.MinValue"/> is the highest priority.
        /// </summary>
        int ConflictResolutionPriority { get; }

        /// <summary>
        /// Gets an optional internal non-localized name that represents the subtype of token.
        /// </summary>
        string? Subtype { get; }

        /// <summary>
        /// Gets a string representation of the data that will be displayed to the user.
        /// </summary>
        string GetDisplayText(string culture);

        bool IsOfSubtype(string expectedSubtype);

        IData MergeDataLocations(IData otherData);
    }

    public interface IData<T> : IData
    {
        T Value { get; }
    }
}
