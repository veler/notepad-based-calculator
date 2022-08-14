namespace NotepadBasedCalculator.BuiltInPlugins.Statements.Header
{
    internal sealed class HeaderStatement : Statement
    {
        internal HeaderStatement(LinkedToken firstToken, LinkedToken lastToken)
            : base(firstToken, lastToken)
        {
        }

        public override string ToString()
        {
            return "Header";
        }
    }
}
