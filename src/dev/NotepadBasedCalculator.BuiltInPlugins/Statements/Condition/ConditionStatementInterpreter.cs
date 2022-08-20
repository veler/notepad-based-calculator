namespace NotepadBasedCalculator.BuiltInPlugins.Statements.Condition
{
    [Export(typeof(IStatementInterpreter))]
    [SupportedStatementType(typeof(ConditionStatement))]
    internal class ConditionStatementInterpreter : IStatementInterpreter
    {
        public async Task<IData?> InterpretStatementAsync(
            string culture,
            IVariableService variableService,
            IExpressionInterpreter expressionInterpreter,
            Statement statement,
            CancellationToken cancellationToken)
        {
            if (statement is not ConditionStatement conditionStatement)
            {
                return null;
            }

            IData? value
                = await expressionInterpreter.InterpretExpressionAsync(
                    culture,
                    variableService,
                    expressionInterpreter,
                    conditionStatement.Condition,
                    cancellationToken)
                .ConfigureAwait(true);

            return value;
        }
    }
}
