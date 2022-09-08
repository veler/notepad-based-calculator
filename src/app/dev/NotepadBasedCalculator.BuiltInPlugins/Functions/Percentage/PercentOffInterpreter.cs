namespace NotepadBasedCalculator.BuiltInPlugins.Functions.Percentage
{
    [Export(typeof(IFunctionInterpreter))]
    [Name("percentage.percentOff")]
    [Culture(SupportedCultures.English)]
    internal sealed class PercentOffInterpreter : IFunctionInterpreter
    {
        [Import]
        public IArithmeticAndRelationOperationService ArithmeticAndRelationOperationService { get; set; } = null!;

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

            // x - (% of x)
            return Task.FromResult(
                ArithmeticAndRelationOperationService.PerformAlgebraOperation(
                    numericData, BinaryOperatorType.Subtraction, percentageData));
        }
    }
}
