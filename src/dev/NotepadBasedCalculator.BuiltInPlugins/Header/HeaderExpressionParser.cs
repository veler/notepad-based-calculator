namespace NotepadBasedCalculator.BuiltInPlugins.Header
{
    [Export(typeof(IExpressionParser))]
    [Culture(SupportedCultures.Any)]
    internal sealed class HeaderExpressionParser : IExpressionParser
    {
        public bool TryParseExpression(string culture, LinkedToken currentToken, out Expression? expression)
        {
            if (currentToken.Token.Is(PredefinedTokenAndDataTypeNames.SymbolOrPunctuation)
                && currentToken.Token.IsTokenTextEqualTo("#", StringComparison.InvariantCulture))
            {
                LinkedToken? previousToken = currentToken.Previous;
                LinkedToken firstTokenInLine = currentToken;
                while (previousToken is not null)
                {
                    firstTokenInLine = previousToken;
                    if (previousToken.Token.IsNot(PredefinedTokenAndDataTypeNames.Whitespace))
                    {
                        expression = null;
                        return false;
                    }

                    previousToken = previousToken.Previous;
                }

                LinkedToken? nextToken = currentToken.Next;
                LinkedToken lastTokenInLine = currentToken;
                while (nextToken is not null)
                {
                    lastTokenInLine = nextToken;
                    nextToken = nextToken.Next;
                }

                expression = new HeaderExpression(firstTokenInLine, lastTokenInLine);
                return true;
            }

            expression = null;
            return false;
        }
    }
}
