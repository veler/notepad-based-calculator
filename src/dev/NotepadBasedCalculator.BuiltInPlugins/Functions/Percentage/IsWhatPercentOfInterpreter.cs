namespace NotepadBasedCalculator.BuiltInPlugins.Functions.Percentage
{
    [Export(typeof(IFunctionInterpreter))]
    [Export]
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

            // ((62.5 * 100) / 250) / 100 = 0.25
            // So 62.5 represents 25% of 250.

            IData? firstNumericDataMultipliedPerOneHundred
                = OperationHelper.PerformAlgebraOperation(
                    firstNumericData,
                    BinaryOperatorType.Multiply,
                    new DecimalData(
                        firstNumericData.LineTextIncludingLineBreak,
                        firstNumericData.StartInLine,
                        firstNumericData.EndInLine,
                        100));

            var percentageData
                = OperationHelper.PerformAlgebraOperation(
                    firstNumericDataMultipliedPerOneHundred,
                    BinaryOperatorType.Division,
                    secondNumericData) as INumericData;

            if (percentageData is not null)
            {
                return Task.FromResult(
                    (IData?)new PercentageData(
                        percentageData.LineTextIncludingLineBreak,
                        percentageData.StartInLine,
                        percentageData.EndInLine,
                        percentageData.ToStandardUnit().NumericValue / 100));
            }

            return Task.FromResult<IData?>(null);
        }
    }
}
