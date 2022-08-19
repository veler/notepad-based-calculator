namespace NotepadBasedCalculator.BuiltInPlugins.Expressions
{
    [Export(typeof(IExpressionInterpreter))]
    [SupportedExpressionType(typeof(BinaryOperatorExpression))]
    internal sealed class BinaryOperatorExpressionInterpreter : IExpressionInterpreter
    {
        private readonly IEnumerable<Lazy<IDataBinaryOperationInterpreter, InterpreterMetadata>> _binaryOperatorsInterpreters;

        [ImportingConstructor]
        public BinaryOperatorExpressionInterpreter(
            [ImportMany] IEnumerable<Lazy<IDataBinaryOperationInterpreter, InterpreterMetadata>> binaryOperatorsInterpreters)
        {
            _binaryOperatorsInterpreters = binaryOperatorsInterpreters;
        }

        public async Task<IData?> InterpretExpressionAsync(
            string culture,
            IVariableService variableService,
            IExpressionInterpreter expressionInterpreter,
            Expression expression,
            CancellationToken cancellationToken)
        {
            if (expression is not BinaryOperatorExpression binaryOperatorExpression)
            {
                return null;
            }

            IData? leftData
                = await expressionInterpreter.InterpretExpressionAsync(
                    culture,
                    variableService,
                    expressionInterpreter,
                    binaryOperatorExpression.LeftExpression,
                    cancellationToken)
                .ConfigureAwait(true);

            IData? rightData
                = await expressionInterpreter.InterpretExpressionAsync(
                    culture,
                    variableService,
                    expressionInterpreter,
                    binaryOperatorExpression.RightExpression,
                    cancellationToken)
                .ConfigureAwait(true);

            if (leftData is not null)
            {
                Type leftDataType = leftData.GetType();
                IDataBinaryOperationInterpreter binaryOperationInterpreter = GetApplicableBinaryOperatorInterpreter(leftDataType, culture);

                return binaryOperationInterpreter.PerformOperation(leftData, binaryOperatorExpression.Operator, rightData);
            }

            return null;
        }

        private IDataBinaryOperationInterpreter GetApplicableBinaryOperatorInterpreter(Type type, string culture)
        {
            return _binaryOperatorsInterpreters.Where(
                    p => p.Metadata.CultureCodes.Any(c => CultureHelper.IsCultureApplicable(c, culture))
                        && p.Metadata.Types.Any(t => t == type))
                    .Single().Value;
        }
    }
}
