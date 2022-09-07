namespace NotepadBasedCalculator.Api
{
    /// <summary>
    /// Basic class that represents a statement.
    /// </summary>
    public abstract class Statement : AbstractSyntaxTreeBase
    {
        protected Statement(LinkedToken firstToken, LinkedToken lastToken)
            : base(firstToken, lastToken)
        {
        }
    }
}
