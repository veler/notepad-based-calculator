namespace NotepadBasedCalculator.BuiltInPlugins.Statements.VariableDeclaration
{
    [Export(typeof(IStatementInterpreter))]
    [SupportedStatementType(typeof(VariableDeclarationStatement))]
    internal class VariableDeclarationStatementInterpreter : IStatementInterpreter
    {
        public async Task<IData?> InterpretStatementAsync(
            string culture,
            IVariableService variableService,
            IExpressionInterpreter expressionInterpreter,
            Statement statement,
            CancellationToken cancellationToken)
        {
            if (statement is not VariableDeclarationStatement variableDeclarationStatement)
            {
                return null;
            }

            IData? value
                = await expressionInterpreter.InterpretExpressionAsync(
                    culture,
                    variableService,
                    expressionInterpreter,
                    variableDeclarationStatement.AssignedValue,
                    cancellationToken)
                .ConfigureAwait(true);

            variableService.SetVariableValue(variableDeclarationStatement.VariableName, value);

            return value;
        }
    }
}
