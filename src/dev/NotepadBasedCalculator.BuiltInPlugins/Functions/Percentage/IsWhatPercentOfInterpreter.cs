namespace NotepadBasedCalculator.BuiltInPlugins.Functions.Percentage
{
    [Export(typeof(IFunctionInterpreter))]
    [Name("percentage.isWhatPercentOf")]
    [Culture(SupportedCultures.English)]
    [Shared]
    internal sealed class IsWhatPercentOfInterpreter : IFunctionInterpreter
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

            IData? firstNumericDataMultipliedPerOneHundred
                = OperationHelper.PerformAlgebraOperation(
                    firstNumericData,
                    BinaryOperatorType.Multiply,
                    new DecimalData(
                        firstNumericData.LineTextIncludingLineBreak,
                        firstNumericData.StartInLine,
                        firstNumericData.EndInLine,
                        100));

            return Task.FromResult(
                OperationHelper.PerformAlgebraOperation(
                    firstNumericDataMultipliedPerOneHundred,
                    BinaryOperatorType.Division,
                    secondNumericData));
        }
    }
}
