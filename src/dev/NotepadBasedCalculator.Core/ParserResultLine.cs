namespace NotepadBasedCalculator.Core
{
    internal sealed class ParserResultLine
    {
        internal TokenizedTextLine TokenizedTextLine { get; }

        internal IReadOnlyList<IData> Data { get; }

        internal IReadOnlyList<Expression> Expressions { get; }

        internal ParserResultLine(TokenizedTextLine tokenizedTextLine, IReadOnlyList<IData> data, IReadOnlyList<Expression> expressions)
        {
            Guard.IsNotNull(tokenizedTextLine);
            Guard.IsNotNull(data);
            Guard.IsNotNull(expressions);
            TokenizedTextLine = tokenizedTextLine;
            Data = data;
            Expressions = expressions;
        }
    }
}
