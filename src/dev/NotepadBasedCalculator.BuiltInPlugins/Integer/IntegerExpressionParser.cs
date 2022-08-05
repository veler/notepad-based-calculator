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
            if (currentToken.Token.Type == TokenType.Number)
            {

            }

            throw new NotImplementedException();
        }
    }
}
