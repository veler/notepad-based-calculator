namespace NotepadBasedCalculator.Api
{
    /// <summary>
    /// Basic class that represents an expression in a statement.
    /// </summary>
    public abstract class Expression : AbstractSyntaxTreeBase
    {
        protected Expression(LinkedToken firstToken, LinkedToken lastToken)
            : base(firstToken, lastToken)
        {
        }
    }
}
