namespace NotepadBasedCalculator.BuiltInPlugins.Functions.Percentage
{
    [Export(typeof(IFunctionInterpreter))]
    [Name("percentage.percentOn")]
    [Culture(SupportedCultures.English)]
    [Shared]
    internal sealed class PercentOnInterpreter : IFunctionInterpreter
    {
        public Task<IData?> InterpretFunctionAsync(
            string culture,
            FunctionDefinition functionDefinition,
            IReadOnlyList<IData> detectedData,
            CancellationToken cancellationToken)
        {
            Guard.HasSizeEqualTo(detectedData, 2);
            detectedData[0].IsOfSubtype("percentage");
            detectedData[1].IsOfType("numeric");
            var percentageData = (INumericData)detectedData[0];
            var numericData = (INumericData)detectedData[1];

            // x + (% of x)
            return Task.FromResult(
                OperationHelper.PerformAlgebraOperation(
                    numericData, BinaryOperatorType.Addition, percentageData));
        }
    }
}
