namespace NotepadBasedCalculator.BuiltInPlugins.Statements.Comment
{
    internal sealed class CommentStatement : Statement
    {
        internal CommentStatement(LinkedToken firstToken, LinkedToken lastToken)
            : base(firstToken, lastToken)
        {
        }

        public override string ToString()
        {
            return "Comment";
        }
    }
}
