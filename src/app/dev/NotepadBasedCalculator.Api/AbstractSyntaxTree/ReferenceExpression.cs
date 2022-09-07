namespace NotepadBasedCalculator.Api
{
    /// <summary>
    /// Basic class for a reference to something.
    /// </summary>
    public abstract class ReferenceExpression : Expression
    {
        protected ReferenceExpression(LinkedToken firstToken, LinkedToken lastToken)
            : base(firstToken, lastToken)
        {
        }
    }
}
