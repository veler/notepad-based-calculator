namespace NotepadBasedCalculator.BuiltInPlugins.Statements.NumericalCalculus
{
    [Export(typeof(IStatementInterpreter))]
    [SupportedStatementType(typeof(NumericalCalculusStatement))]
    internal class NumericalCalculusStatementInterpreter : IStatementInterpreter
    {
        public async Task<IData?> InterpretStatementAsync(
            string culture,
            IVariableService variableService,
            IExpressionInterpreter expressionInterpreter,
            Statement statement,
            CancellationToken cancellationToken)
        {
            if (statement is not NumericalCalculusStatement numericalCalculusStatement)
            {
                return null;
            }

            IData? value
                = await expressionInterpreter.InterpretExpressionAsync(
                    culture,
                    variableService,
                    expressionInterpreter,
                    numericalCalculusStatement.NumericalCalculusExpression,
                    cancellationToken)
                .ConfigureAwait(true);

            if (value is not null)
            {
                // We do this to give a change to the data to convert itself into a numeric value, if needed.
                value = value.MergeDataLocations(value);
            }

            return value;
        }
    }
}
