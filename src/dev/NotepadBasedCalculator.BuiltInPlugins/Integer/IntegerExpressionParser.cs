using NotepadBasedCalculator.Api;
using NotepadBasedCalculator.Api.AbstractSyntaxTree;

namespace NotepadBasedCalculator.BuiltInPlugins.Integer
{
    [Export(typeof(IExpressionParser))]
    [Shared]
    public class IntegerExpressionParser : IExpressionParser
    {
        public bool TryParseExpression(
            LinkedToken currentToken,
            out Expression? expression)
        {
            bool isNegativeNumber = false;

            if (currentToken.Token.Type == TokenType.SymbolOrPunctuation
                && currentToken.Next is not null
                && currentToken.Next.Token.Type == TokenType.Number
                && currentToken.Token.IsTokenTextEqualTo("-", StringComparison.Ordinal))
            {
                isNegativeNumber = true;
                currentToken = currentToken.Next;
            }

            if (currentToken.Token.Type == TokenType.Number)
            {
                return true;
            }

            expression = null;
            return false;
        }
    }
}
