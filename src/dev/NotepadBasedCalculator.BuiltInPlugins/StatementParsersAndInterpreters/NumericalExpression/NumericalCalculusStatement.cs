namespace NotepadBasedCalculator.BuiltInPlugins.StatementParsersAndInterpreters.NumericalExpression
{
    internal sealed class NumericalCalculusStatement : Statement
    {
        internal Expression NumericalCalculusExpression { get; }

        internal NumericalCalculusStatement(LinkedToken firstToken, LinkedToken lastToken, Expression numericalCalculusExpression)
            : base(firstToken, lastToken)
        {
            NumericalCalculusExpression = numericalCalculusExpression;
        }

        public override string ToString()
        {
            return NumericalCalculusExpression.ToString();
        }
    }
}
