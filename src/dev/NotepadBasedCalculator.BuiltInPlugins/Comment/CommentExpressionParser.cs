namespace NotepadBasedCalculator.BuiltInPlugins.Comment
{
    [Export(typeof(IStatementParser))]
    [Order(0)]
    [Culture(SupportedCultures.Any)]
    internal class CommentExpressionParser : IStatementParser
    {
        public override bool TryParseExpression(string culture, LinkedToken currentToken, out Expression? expression)
        {
            if (currentToken.Token.Type == PredefinedTokenAndDataTypeNames.SymbolOrPunctuation
                && currentToken.Token.IsTokenTextEqualTo("/", StringComparison.InvariantCulture)
                && currentToken.Next is not null
                && currentToken.Next.Token.Type == PredefinedTokenAndDataTypeNames.SymbolOrPunctuation
                && currentToken.Next.Token.IsTokenTextEqualTo("/", StringComparison.InvariantCulture))
            {
                LinkedToken? nextToken = currentToken.Next;
                LinkedToken lastTokenInLine = currentToken;
                while (nextToken is not null)
                {
                    lastTokenInLine = nextToken;
                    nextToken = nextToken.Next;
                }

                expression = new CommentExpression(currentToken, lastTokenInLine);
                return true;
            }

            expression = null;
            return false;
        }
    }
}
