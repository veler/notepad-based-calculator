namespace NotepadBasedCalculator.Core
{
    internal sealed class ParserAndInterpreterResultUpdatedEventArgs : DeferredCancelEventArgs
    {
        internal IReadOnlyList<ParserAndInterpreterResultLine>? ResultPerLines { get; }

        public ParserAndInterpreterResultUpdatedEventArgs(IReadOnlyList<ParserAndInterpreterResultLine>? resultPerLines)
        {
            ResultPerLines = resultPerLines;
        }
    }
}
