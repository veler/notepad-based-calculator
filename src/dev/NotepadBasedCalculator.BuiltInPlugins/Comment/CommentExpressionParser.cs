namespace NotepadBasedCalculator.BuiltInPlugins.Comment
{
    [Export(typeof(IExpressionParser))]
    [Order(0)]
    [Culture(SupportedCultures.Any)]
    internal sealed class CommentExpressionParser : ParserBase, IExpressionParser
    {
        public bool TryParseExpression(string culture, LinkedToken currentToken, out Expression? expression)
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

                expression = new CommentExpression(DiscardWhitespaces(currentToken)!, lastTokenInLine);
                return true;
            }

            expression = null;
            return false;
        }
    }
}
