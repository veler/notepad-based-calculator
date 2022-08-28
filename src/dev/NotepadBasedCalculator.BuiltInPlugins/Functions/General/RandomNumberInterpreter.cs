﻿namespace NotepadBasedCalculator.BuiltInPlugins.Functions.General
{
    [Export(typeof(IFunctionInterpreter))]
    [Name("general.random")]
    [Culture(SupportedCultures.English)]
    [Shared]
    internal sealed class RandomNumberInterpreter : IFunctionInterpreter
    {
        private readonly Random _random = new();

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

            double result = 0;
            double first = firstNumber.ToStandardUnit().NumericValue;
            double second = secondNumber.ToStandardUnit().NumericValue;

            if ((first == 0 && second == 1) || (first == 1 && second == 0))
            {
                result = _random.NextDouble();
            }
            else if (first >= second)
            {
                result = _random.Next((int)second, (int)first);
            }
            else if (first < second)
            {
                result = _random.Next((int)first, (int)second);
            }

            return Task.FromResult((IData?)firstNumber.FromStandardUnit(result).MergeDataLocations(secondNumber));
        }
    }
}
