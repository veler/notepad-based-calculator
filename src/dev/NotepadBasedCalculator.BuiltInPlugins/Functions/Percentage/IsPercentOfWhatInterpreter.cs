namespace NotepadBasedCalculator.BuiltInPlugins.Functions.Percentage
{
    [Export(typeof(IFunctionInterpreter))]
    [Name("percentage.isPercentOfWhat")]
    [Culture(SupportedCultures.English)]
    [Shared]
    internal sealed class IsPercentOfWhatInterpreter : IFunctionInterpreter
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

            PercentageData percentageData;
            INumericData numericData;

            if (detectedData[0].IsOfSubtype("percentage")
                && detectedData[1].IsOfType("numeric"))
            {
                percentageData = (PercentageData)detectedData[0];
                numericData = (INumericData)detectedData[1];
            }
            else if (detectedData[0].IsOfType("numeric")
                && detectedData[1].IsOfSubtype("percentage"))
            {
                numericData = (INumericData)detectedData[0];
                percentageData = (PercentageData)detectedData[1];
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
                return null;
            }

            // 250 * (1 / 0.25)
            // 250 * 4
            // = 1000
            // so 250 is 25% of 1000.
            // also, 250 is 25% off 1000.
            return Task.FromResult(
                OperationHelper.PerformAlgebraOperation(
                    numericData,
                    BinaryOperatorType.Multiply,
                    numericData.FromStandardUnit(1 / percentageData.NumericValueInStandardUnit)));
        }
    }
}
