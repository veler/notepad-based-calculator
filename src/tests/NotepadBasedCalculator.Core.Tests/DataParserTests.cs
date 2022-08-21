using System.Threading.Tasks;
using NotepadBasedCalculator.Api;
using Xunit;

namespace NotepadBasedCalculator.Core.Tests
{
    public sealed class DataParserTests : MefBaseTest
    {
        [Theory]
        [InlineData("thirty five thousand", 35000)]
        [InlineData("forty three thousand", 43000)]
        [InlineData("nine hundred and seventy four thousand", 974000)]
        [InlineData("I have nine hundred and seventy four thousand items", 974000)]
        public async Task IntegerParsingAsync(string input, int output)
        {
            Parser parser = ExportProvider.Import<Parser>();
            ParserResult parserResult = await parser.ParseAsync(input);
            Assert.Single(parserResult.Lines[0].Data);
            Assert.True(parserResult.Lines[0].Data[0].Is(PredefinedTokenAndDataTypeNames.Numeric));
            Assert.Equal(PredefinedTokenAndDataTypeNames.SubDataTypeNames.Decimal, parserResult.Lines[0].Data[0].Subtype);
            Assert.Equal(output, ((DecimalData)parserResult.Lines[0].Data[0]).Value);
        }

        [Theory]
        [InlineData("thirty five thousand point one two three", 35000.123)]
        [InlineData("forty three thousand point fifty seven", 43000.57)]
        [InlineData("1.1^+23", 8.95430243255239)]
        public async Task DecimalParsingAsync(string input, float output)
        {
            Parser parser = ExportProvider.Import<Parser>();
            ParserResult parserResult = await parser.ParseAsync(input);
            Assert.Single(parserResult.Lines[0].Data);
            Assert.True(parserResult.Lines[0].Data[0].Is(PredefinedTokenAndDataTypeNames.Numeric));
            Assert.Equal(PredefinedTokenAndDataTypeNames.SubDataTypeNames.Decimal, parserResult.Lines[0].Data[0].Subtype);
            Assert.Equal(output, ((DecimalData)parserResult.Lines[0].Data[0]).Value);
        }

        [Theory]
        [InlineData("a fifth", 0.2)]
        [InlineData("a hundred thousand trillionths", 1E-07)]
        public async Task FractionParsingAsync(string input, float output)
        {
            Parser parser = ExportProvider.Import<Parser>();
            ParserResult parserResult = await parser.ParseAsync(input);
            Assert.Single(parserResult.Lines[0].Data);
            Assert.True(parserResult.Lines[0].Data[0].Is(PredefinedTokenAndDataTypeNames.Numeric));
            Assert.Equal(PredefinedTokenAndDataTypeNames.SubDataTypeNames.Fraction, parserResult.Lines[0].Data[0].Subtype);
            Assert.Equal(output, ((FractionData)parserResult.Lines[0].Data[0]).Value);
        }

        [Theory]
        [InlineData("one hundred percents", 1f)]
        [InlineData("per cent of twenty-two", 0.22)]
        public async Task PercentageParsingAsync(string input, float output)
        {
            Parser parser = ExportProvider.Import<Parser>();
            ParserResult parserResult = await parser.ParseAsync(input);
            Assert.Single(parserResult.Lines[0].Data);
            Assert.True(parserResult.Lines[0].Data[0].Is(PredefinedTokenAndDataTypeNames.Numeric));
            Assert.Equal(PredefinedTokenAndDataTypeNames.SubDataTypeNames.Percentage, parserResult.Lines[0].Data[0].Subtype);
            Assert.Equal(output, ((PercentageData)parserResult.Lines[0].Data[0]).Value);
        }

        [Theory]
        [InlineData("one hundred and fifty thousand dollars", 150000, "Dollar", "", false)]
        [InlineData(" $ -75.3 million USD", -75300000, "United States dollar", "USD", true)]
        public async Task CurrencyParsingAsync(string input, float value, string unit, string iso, bool isNegative)
        {
            Parser parser = ExportProvider.Import<Parser>();
            ParserResult parserResult = await parser.ParseAsync(input);
            Assert.Single(parserResult.Lines[0].Data);
            Assert.True(parserResult.Lines[0].Data[0].Is(PredefinedTokenAndDataTypeNames.Numeric));
            CurrencyValue currency = ((CurrencyData)parserResult.Lines[0].Data[0]).Value;
            Assert.Equal(isNegative, ((CurrencyData)parserResult.Lines[0].Data[0]).IsNegative);
            Assert.Equal(value, currency.Value);
            Assert.Equal(unit, currency.Currency);
            Assert.Equal(iso, currency.IsoCurrency);
        }

        [Theory]
        [InlineData("three kilometers", 3, "Length", "Kilometer", false)]
        [InlineData("three km/h", 3, "Speed", "Kilometer per hour", false)]
        public async Task DimensionParsingAsync(string input, float value, string subType, string unit, bool isNegative)
        {
            Parser parser = ExportProvider.Import<Parser>();
            ParserResult parserResult = await parser.ParseAsync(input);
            Assert.Single(parserResult.Lines[0].Data);
            Assert.True(parserResult.Lines[0].Data[0].Is(PredefinedTokenAndDataTypeNames.Numeric));
            Assert.Equal(subType, ((UnitData)parserResult.Lines[0].Data[0]).Subtype);
            UnitFloat unitFloat = ((UnitData)parserResult.Lines[0].Data[0]).Value;
            Assert.Equal(isNegative, ((UnitData)parserResult.Lines[0].Data[0]).IsNegative);
            Assert.Equal(value, unitFloat.Value);
            Assert.Equal(unit, unitFloat.Unit);
        }
    }
}
