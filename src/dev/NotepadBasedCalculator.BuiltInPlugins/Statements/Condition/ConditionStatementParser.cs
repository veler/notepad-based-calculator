namespace NotepadBasedCalculator.BuiltInPlugins.Statements.Condition
{
    [Export(typeof(IStatementParser))]
    [Culture(SupportedCultures.Any)]
    internal sealed class ConditionStatementParser : ParserBase, IStatementParser
    {
        public bool TryParseStatement(string culture, LinkedToken currentToken, out Statement? statement)
        {
            if (DiscardIf(currentToken, out currentToken))
            {
                Expression? expression = ParseExpression(culture, currentToken, out LinkedToken? nextToken);

                if (expression is not null && DiscardThen(nextToken, out nextToken))
                {

                }
            }

            statement = null;
            return false;
        }

        private bool DiscardIf(LinkedToken currentToken, out LinkedToken nextToken)
        {
            return DiscardToken(
                currentToken,
                PredefinedTokenAndDataTypeNames.Word,
                "if",
                StringComparison.OrdinalIgnoreCase,
                ignoreUnknownWords: true,
                out nextToken!)
                && nextToken is not null;
        }

        private bool DiscardThen(LinkedToken? currentToken, out LinkedToken? nextToken)
        {
            nextToken = currentToken;
            do
            {
                if (DiscardToken(
                    nextToken,
                    PredefinedTokenAndDataTypeNames.Word,
                 "then",
                    StringComparison.OrdinalIgnoreCase,
                    ignoreUnknownWords: true,
                    out nextToken!)
                    && nextToken is not null)
                {
                    return true;
                }
            } while (nextToken is not null);

            return false;
        }
    }
}
