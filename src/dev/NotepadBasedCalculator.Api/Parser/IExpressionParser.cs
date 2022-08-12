namespace NotepadBasedCalculator.Api
{
    public interface IExpressionParser
    {
        bool TryParseExpression(string culture, LinkedToken currentToken, out Expression? expression);
    }
}
