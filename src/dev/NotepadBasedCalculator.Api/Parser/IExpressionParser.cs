namespace NotepadBasedCalculator.Api
{
    public interface IExpressionParser
    {
        bool TryParseExpression(
            LinkedToken currentToken,
            string culture,
            out Expression? expression);
    }
}
