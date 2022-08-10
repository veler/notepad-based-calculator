namespace NotepadBasedCalculator.Api
{
    public abstract class IStatementParser
    {
        public virtual bool TryParseExpression(string culture, LinkedToken currentToken, out Expression? expression)
        {
            expression = null;
            return false;
        }

        public virtual bool TryParseExpression(string culture, string lineText, out Expression? expression)
        {
            expression = null;
            return false;
        }
    }
}
