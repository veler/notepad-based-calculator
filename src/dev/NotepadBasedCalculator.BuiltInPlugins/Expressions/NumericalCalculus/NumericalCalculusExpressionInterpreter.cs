namespace NotepadBasedCalculator.BuiltInPlugins.Expressions.NumericalCalculus
{
    [Export(typeof(IExpressionInterpreter))]
    [SupportedExpressionType(typeof(DataExpression))]
    [SupportedExpressionType(typeof(VariableReferenceExpression))]
    [SupportedExpressionType(typeof(GroupExpression))]
    internal sealed class NumericalCalculusExpressionInterpreter : IExpressionInterpreter
    {
        public Task<IData?> InterpretExpressionAsync(
            string culture,
            IVariableService variableService,
            IExpressionInterpreter expressionInterpreter,
            Expression expression,
            CancellationToken cancellationToken)
        {
            if (expression is DataExpression dataExpression)
            {
                return Task.FromResult<IData?>(dataExpression.Data);
            }

            if (expression is VariableReferenceExpression variableReferenceExpression)
            {
                return Task.FromResult(variableService.GetVariableValue(variableReferenceExpression.VariableName));
            }

            throw new NotImplementedException();
        }
    }
}
