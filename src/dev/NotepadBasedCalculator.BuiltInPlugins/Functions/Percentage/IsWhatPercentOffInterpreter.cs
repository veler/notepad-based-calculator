namespace NotepadBasedCalculator.BuiltInPlugins.Functions.Percentage
{
    [Export(typeof(IFunctionInterpreter))]
    [Name("percentage.isWhatPercentOff")]
    [Culture(SupportedCultures.English)]
    [Shared]
    internal sealed class IsWhatPercentOffInterpreter : IFunctionInterpreter
    {
        [Import]
        public IsWhatPercentOfInterpreter IsWhatPercentOfInterpreter { get; set; } = null!;

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

            var one
                = new DecimalData(
                    firstNumericData.LineTextIncludingLineBreak,
                    firstNumericData.StartInLine,
                    firstNumericData.EndInLine,
                    1);

            // 1 - (1 / (250 / 62.5)) = 0.75
            // so 62.5 represents 250 - 75% (aka. 75% off 25).
            return Task.FromResult(
                OperationHelper.PerformAlgebraOperation(
                    one,
                    BinaryOperatorType.Subtraction,
                    OperationHelper.PerformAlgebraOperation(
                        one,
                        BinaryOperatorType.Division,
                        OperationHelper.PerformAlgebraOperation(
                            secondNumericData,
                            BinaryOperatorType.Division,
                            firstNumericData))));
        }
    }
}
