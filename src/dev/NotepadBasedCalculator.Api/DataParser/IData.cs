namespace NotepadBasedCalculator.Api
{
    public interface IData : IToken, IEquatable<IData>, IComparable<IData>
    {
        /// <summary>
        /// Gets an optional internal non-localized name that represents the subtype of token.
        /// </summary>
        string? Subtype { get; }

        /// <summary>
        /// Gets a string representation of the data that will be displayed to the user.
        /// </summary>
        string DisplayText { get; }

        IData MergeDataLocations(IData otherData);
    }

    public interface IData<T> : IData
    {
        T Value { get; }
    }
}
