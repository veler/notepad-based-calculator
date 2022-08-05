using NotepadBasedCalculator.Api.AbstractSyntaxTree;

namespace NotepadBasedCalculator.Api
{
    public interface IExpressionParser
    {
        bool TryParseExpression(
            LinkedToken currentToken,
            out Expression? expression);
    }
}
