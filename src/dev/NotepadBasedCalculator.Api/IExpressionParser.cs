using NotepadBasedCalculator.Api.AbstractSyntaxTree;

namespace NotepadBasedCalculator.Api
{
    public interface IExpressionParser
    {
        bool TryParseExpression(
            LinkedToken currentToken,
            CultureInfo cultureInfo,
            out Expression? expression);
    }
}
