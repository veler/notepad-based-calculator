namespace NotepadBasedCalculator.Core
{
    internal sealed class ParserAndInterpreterResult
    {
        internal IReadOnlyList<ParserResultLine> Lines { get; }

        internal ParserAndInterpreterResult(IReadOnlyList<ParserResultLine> lines)
        {
            Guard.IsNotNull(lines);
            Lines = lines;
        }
    }
}
