using NotepadBasedCalculator.Api;
using NotepadBasedCalculator.Api.AbstractSyntaxTree;

namespace NotepadBasedCalculator.BuiltInPlugins.Integer
{
    [Export(typeof(IExpressionParser))]
    public sealed class IntegerExpressionParser : IExpressionParser
    {
        public bool TryParseExpression(
            LinkedToken currentToken,
            out Expression? expression)
        {
            LinkedToken firstToken = currentToken;
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
                string numberString;
                if (isNegativeNumber)
                {
                    numberString = "-" + currentToken.Token.GetText();
                }
                else
                {
                    numberString = currentToken.Token.GetText();
                }

                if (int.TryParse(numberString, out int number))
                {
                    expression = new IntegerExpression(firstToken, lastToken: currentToken, number);
                    return true;
                }
            }

            expression = null;
            return false;
        }
    }
}
