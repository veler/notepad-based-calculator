namespace NotepadBasedCalculator.Core
{
    internal sealed class ParserResultLine
    {
        internal TokenizedTextLine TokenizedTextLine { get; }

        internal IReadOnlyList<IData> Data { get; }

        internal ParserResultLine(TokenizedTextLine tokenizedTextLine, IReadOnlyList<IData> data)
        {
            Guard.IsNotNull(tokenizedTextLine);
            Guard.IsNotNull(data);
            TokenizedTextLine = tokenizedTextLine;
            Data = data;
        }
    }
}
