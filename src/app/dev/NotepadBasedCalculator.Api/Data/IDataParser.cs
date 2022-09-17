namespace NotepadBasedCalculator.Api
{
    /// <summary>
    /// Provides a way to extract a data out from a line of text such as a number or a date.
    /// </summary>
    public interface IDataParser
    {
        /// <summary>
        /// Parses the data in the given <paramref name="tokenizedTextLine"/>.
        /// </summary>
        IReadOnlyList<IData>? Parse(string culture, TokenizedTextLine tokenizedTextLine, CancellationToken cancellationToken);
    }
}
