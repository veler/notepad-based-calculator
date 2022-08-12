namespace NotepadBasedCalculator.Api
{
    public interface IData : IToken, IEquatable<IData>, IComparable<IData>
    {
        /// <summary>
        /// Gets an optional internal non-localized name that represents the subtype of token.
        /// </summary>
        public string? Subtype { get; }
    }

    public interface IData<T> : IData
    {
        T Value { get; }
    }
}
