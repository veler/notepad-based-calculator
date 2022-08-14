namespace NotepadBasedCalculator.BuiltInPlugins.Statements.Comment
{
    [Export(typeof(IStatementParser))]
    [Order(0)]
    [Culture(SupportedCultures.Any)]
    internal sealed class CommentStatementParser : ParserBase, IStatementParser
    {
        public bool TryParseStatement(string culture, LinkedToken currentToken, out Statement? expression)
        {
            if (DiscardToken(currentToken, PredefinedTokenAndDataTypeNames.SymbolOrPunctuation, "/", StringComparison.InvariantCulture, out LinkedToken? nextToken)
                && nextToken is not null
                && DiscardToken(nextToken, PredefinedTokenAndDataTypeNames.SymbolOrPunctuation, "/", StringComparison.InvariantCulture, ignoreWhitespace: false, out nextToken))
            {
                LinkedToken lastTokenInLine = currentToken;
                while (nextToken is not null)
                {
                    lastTokenInLine = nextToken;
                    nextToken = nextToken.Next;
                }

                expression = new CommentStatement(DiscardWhitespaces(currentToken)!, lastTokenInLine);
                return true;
            }

            expression = null;
            return false;
        }
    }
}
