namespace NotepadBasedCalculator.Core
{
    internal sealed class ParserResult
    {
        internal IReadOnlyList<ParserResultLine> Lines { get; }

        internal ParserResult(IReadOnlyList<ParserResultLine> lines)
        {
            Guard.IsNotNull(lines);
            Lines = lines;
        }
    }
}
