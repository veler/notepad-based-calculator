using NotepadBasedCalculator.Api;
using NotepadBasedCalculator.Api.AbstractSyntaxTree;

namespace NotepadBasedCalculator.BuiltInPlugins.Comment
{
    [Export(typeof(IExpressionParser))]
    [Order(0)]
    internal class CommentExpressionParser : IExpressionParser
    {
        public bool TryParseExpression(LinkedToken currentToken, CultureInfo cultureInfo, out Expression? expression)
        {
            if (currentToken.Token.Type == TokenType.SymbolOrPunctuation
                && currentToken.Token.IsTokenTextEqualTo("/", StringComparison.InvariantCulture)
                && currentToken.Next is not null
                && currentToken.Next.Token.Type == TokenType.SymbolOrPunctuation
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
