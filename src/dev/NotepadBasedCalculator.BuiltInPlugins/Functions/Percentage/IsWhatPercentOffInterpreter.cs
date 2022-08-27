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

            return IsWhatPercentOfInterpreter.InterpretFunctionAsync(
                   culture,
                   functionDefinition,
                   new[]
                   {
                       firstNumericData,
                       OperationHelper.PerformAlgebraOperation(secondNumericData, BinaryOperatorType.Subtraction, firstNumericData)!
                   },
                   cancellationToken);
        }
    }
}
