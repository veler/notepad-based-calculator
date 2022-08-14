namespace NotepadBasedCalculator.Core
{
    internal sealed class ParserResultLine
    {
        internal TokenizedTextLine TokenizedTextLine { get; }

        internal IReadOnlyList<IData> Data { get; }

        internal IReadOnlyList<Statement> Statements { get; }

        internal ParserResultLine(TokenizedTextLine tokenizedTextLine, IReadOnlyList<IData> data, IReadOnlyList<Statement> statements)
        {
            Guard.IsNotNull(tokenizedTextLine);
            Guard.IsNotNull(data);
            Guard.IsNotNull(statements);
            TokenizedTextLine = tokenizedTextLine;
            Data = data;
            Statements = statements;
        }
    }
}
