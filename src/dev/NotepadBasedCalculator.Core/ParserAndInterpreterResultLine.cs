namespace NotepadBasedCalculator.Core
{
    internal sealed class ParserAndInterpreterResultLine
    {
        internal TokenizedTextLine TokenizedTextLine { get; }

        internal IReadOnlyList<StatementParserAndInterpreterResult> StatementsAndData { get; }

        internal IData? SummarizedResultData { get; }

        internal ParserAndInterpreterResultLine(
            TokenizedTextLine tokenizedTextLine,
            IReadOnlyList<StatementParserAndInterpreterResult>? statementsAndData = null,
            IData? summarizedResultData = null)
        {
            Guard.IsNotNull(tokenizedTextLine);
            TokenizedTextLine = tokenizedTextLine;
            StatementsAndData = statementsAndData ?? new List<StatementParserAndInterpreterResult>();
            SummarizedResultData = summarizedResultData;
        }
    }
}
