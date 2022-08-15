namespace NotepadBasedCalculator.BuiltInPlugins.Statements.Comment
{
    [Export(typeof(IStatementParser))]
    [Culture(SupportedCultures.Any)]
    [Order(int.MinValue)]
    internal sealed class CommentStatementParser : ParserBase, IStatementParser
    {
        public bool TryParseStatement(string culture, LinkedToken currentToken, out Statement? expression)
        {
            if (currentToken.Token.Is(PredefinedTokenAndDataTypeNames.SymbolOrPunctuation, "//", StringComparison.InvariantCulture))
            {
                LinkedToken lastTokenInLine = currentToken;
                LinkedToken? nextToken = currentToken.Next;
                while (nextToken is not null)
                {
                    lastTokenInLine = nextToken;
                    nextToken = nextToken.Next;
                }

                expression = new CommentStatement(DiscardWords(currentToken)!, lastTokenInLine);
                return true;
            }

            expression = null;
            return false;
        }
    }
}
