using NotepadBasedCalculator.Api;
using NotepadBasedCalculator.Api.AbstractSyntaxTree;

namespace NotepadBasedCalculator.BuiltInPlugins.Header
{
    [Export(typeof(IExpressionParser))]
    internal sealed class HeaderExpressionParser : IExpressionParser
    {
        public bool TryParseExpression(LinkedToken currentToken, CultureInfo cultureInfo, out Expression? expression)
        {
            if (currentToken.Token.Type == TokenType.SymbolOrPunctuation
                && currentToken.Token.IsTokenTextEqualTo("#", StringComparison.InvariantCulture))
            {
                LinkedToken? previousToken = currentToken.Previous;
                LinkedToken firstTokenInLine = currentToken;
                while (previousToken is not null)
                {
                    firstTokenInLine = previousToken;
                    if (previousToken.Token.Type != TokenType.Whitespace)
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
