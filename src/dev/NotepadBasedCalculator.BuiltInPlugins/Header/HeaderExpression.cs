namespace NotepadBasedCalculator.BuiltInPlugins.Header
{
    internal sealed class HeaderExpression : Expression
    {
        public HeaderExpression(LinkedToken firstToken, LinkedToken lastToken)
            : base(firstToken, lastToken)
        {
        }

        public override string ToString()
        {
            return "Header statement";
        }
    }
}
