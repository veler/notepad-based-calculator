namespace NotepadBasedCalculator.BuiltInPlugins.Functions.Percentage
{
    [Export(typeof(IFunctionInterpreter))]
    [Name("percentage.isPercentOnWhat")]
    [Culture(SupportedCultures.English)]
    [Shared]
    internal sealed class IsPercentOnWhatInterpreter : IFunctionInterpreter
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

            // 250 / (1 + 0.25)
            // 250 / 125%
            // = 200
            // so 250 is 25% on 200.
            return Task.FromResult(
                ArithmeticAndRelationOperationService.PerformAlgebraOperation(
                    numericData,
                    BinaryOperatorType.Division,
                    numericData.CreateFromStandardUnit(1 + percentageData.NumericValueInStandardUnit)));
        }
    }
}
