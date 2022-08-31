namespace NotepadBasedCalculator.BuiltInPlugins.Functions.General
{
    [Export(typeof(IFunctionInterpreter))]
    [Name("general.midpoint")]
    [Culture(SupportedCultures.English)]
    [Shared]
    internal sealed class MidpointInterpreter : IFunctionInterpreter
    {
        public Task<IData?> InterpretFunctionAsync(
            string culture,
            FunctionDefinition functionDefinition,
            IReadOnlyList<IData> detectedData,
            CancellationToken cancellationToken)
        {
            Guard.HasSizeEqualTo(detectedData, 2);
            detectedData[0].IsOfType("numeric");
            detectedData[1].IsOfType("numeric");
            var firstNumber = (INumericData)detectedData[0];
            var secondNumber = (INumericData)detectedData[1];

            if ((firstNumber is IConvertibleNumericData firstConvertibleNumericData && !firstConvertibleNumericData.CanConvertFrom(secondNumber))
                || (secondNumber is IConvertibleNumericData secondConvertibleNumericData && !secondConvertibleNumericData.CanConvertFrom(firstNumber)))
            {
                return Task.FromResult<IData?>(null);
            }

            double first = firstNumber.NumericValueInStandardUnit;
            double second = secondNumber.NumericValueInStandardUnit;
            double result = (first + second) / 2;

            return Task.FromResult((IData?)secondNumber.FromStandardUnit(result).MergeDataLocations(firstNumber));
        }
    }
}
