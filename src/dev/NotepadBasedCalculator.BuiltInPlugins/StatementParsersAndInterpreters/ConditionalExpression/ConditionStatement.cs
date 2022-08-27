namespace NotepadBasedCalculator.BuiltInPlugins.StatementParsersAndInterpreters.ConditionalExpression
{
    internal sealed class ConditionStatement : Statement
    {
        internal Expression Condition { get; }

        // TODO: Then and Else expression.

        internal ConditionStatement(LinkedToken firstToken, LinkedToken lastToken, Expression condition)
            : base(firstToken, lastToken)
        {
            Condition = condition;
        }

        public override string ToString()
        {
            return "Condition";
        }
    }
}
