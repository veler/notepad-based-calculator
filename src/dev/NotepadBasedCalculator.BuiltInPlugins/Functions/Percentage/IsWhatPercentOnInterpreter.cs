namespace NotepadBasedCalculator.BuiltInPlugins.Functions.Percentage
{
    [Export(typeof(IFunctionInterpreter))]
    [Name("percentage.isWhatPercentOn")]
    [Culture(SupportedCultures.English)]
    [Shared]
    internal sealed class IsWhatPercentOnInterpreter : IFunctionInterpreter
    {
        public Task<IData?> InterpretFunctionAsync(
            string culture,
            FunctionDefinition functionDefinition,
            IReadOnlyList<IData> detectedData,
            CancellationToken cancellationToken)
        {
            Guard.HasSizeEqualTo(detectedData, 2);
            detectedData[0].IsOfSubtype("numeric");
            detectedData[1].IsOfType("numeric");
            var firstNumericData = (INumericData)detectedData[0];
            var secondNumericData = (INumericData)detectedData[1];

            return Task.FromResult(
                OperationHelper.PerformAlgebraOperation(
                    secondNumericData,
                    BinaryOperatorType.Addition,
                    new PercentageData(
                        firstNumericData.LineTextIncludingLineBreak,
                        firstNumericData.StartInLine,
                        firstNumericData.EndInLine,
                        firstNumericData.NumericValue / 100)));
        }
    }
}
