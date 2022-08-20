namespace NotepadBasedCalculator.BuiltInPlugins.Expressions.NumericalCalculus
{
    [Export(typeof(IExpressionInterpreter))]
    [SupportedExpressionType(typeof(DataExpression))]
    [SupportedExpressionType(typeof(VariableReferenceExpression))]
    [SupportedExpressionType(typeof(GroupExpression))]
    internal sealed class NumericalCalculusExpressionInterpreter : IExpressionInterpreter
    {
        public async Task<IData?> InterpretExpressionAsync(
            string culture,
            IVariableService variableService,
            IExpressionInterpreter expressionInterpreter,
            Expression expression,
            CancellationToken cancellationToken)
        {
            if (expression is DataExpression dataExpression)
            {
                return dataExpression.Data;
            }

            if (expression is VariableReferenceExpression variableReferenceExpression)
            {
                return variableService.GetVariableValue(variableReferenceExpression.VariableName);
            }

            if (expression is GroupExpression groupExpression)
            {
                return await expressionInterpreter.InterpretExpressionAsync(
                    culture,
                    variableService,
                    expressionInterpreter,
                    groupExpression.InnerExpression,
                    cancellationToken);
            }

            throw new NotImplementedException();
        }
    }
}
