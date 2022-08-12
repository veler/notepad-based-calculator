using System.Threading.Tasks;
using NotepadBasedCalculator.Api;
using Xunit;

namespace NotepadBasedCalculator.Core.Tests
{
    public sealed class DataParser : MefBaseTest
    {
        [Theory]
        [InlineData("thirty five thousand", 35000)]
        [InlineData("forty three thousand", 43000)]
        [InlineData("one hundred and fifty thousand dollars", 150000)]
        [InlineData("nine hundred and seventy four thousand", 974000)]
        [InlineData("I have nine hundred and seventy four thousand items", 974000)]
        public async Task IntegerParsingAsync(string input, int output)
        {
            Parser parser = ExportProvider.Import<Parser>();
            ParserResult parserResult = await parser.ParseAsync(input);
            Assert.Equal(1, parserResult.Lines[0].Data.Count);
            Assert.Equal(PredefinedTokenAndDataTypeNames.Numeric, parserResult.Lines[0].Data[0].Type);
            Assert.Equal(PredefinedTokenAndDataTypeNames.SubDataTypeNames.Integer, parserResult.Lines[0].Data[0].Subtype);
            Assert.Equal(output, ((IntegerData)parserResult.Lines[0].Data[0]).Value);
        }

        [Theory]
        [InlineData("thirty five thousand point one two three", 35000.123)]
        [InlineData("forty three thousand point fifty seven", 43000.57)]
        [InlineData("1.1^+23", 8.95430243255239)]
        public async Task DecimalParsingAsync(string input, float output)
        {
            Parser parser = ExportProvider.Import<Parser>();
            ParserResult parserResult = await parser.ParseAsync(input);
            Assert.Equal(1, parserResult.Lines[0].Data.Count);
            Assert.Equal(PredefinedTokenAndDataTypeNames.Numeric, parserResult.Lines[0].Data[0].Type);
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
            Assert.Equal(1, parserResult.Lines[0].Data.Count);
            Assert.Equal(PredefinedTokenAndDataTypeNames.Numeric, parserResult.Lines[0].Data[0].Type);
            Assert.Equal(PredefinedTokenAndDataTypeNames.SubDataTypeNames.Fraction, parserResult.Lines[0].Data[0].Subtype);
            Assert.Equal(output, ((FractionData)parserResult.Lines[0].Data[0]).Value);
        }

        [Theory]
        [InlineData("one hundred percents", 100)]
        [InlineData("per cent of twenty-two", 22)]
        public async Task PercentageParsingAsync(string input, float output)
        {
            Parser parser = ExportProvider.Import<Parser>();
            ParserResult parserResult = await parser.ParseAsync(input);
            Assert.Equal(1, parserResult.Lines[0].Data.Count);
            Assert.Equal(PredefinedTokenAndDataTypeNames.Numeric, parserResult.Lines[0].Data[0].Type);
            Assert.Equal(PredefinedTokenAndDataTypeNames.SubDataTypeNames.Percentage, parserResult.Lines[0].Data[0].Subtype);
            Assert.Equal(output, ((PercentageData)parserResult.Lines[0].Data[0]).Value);
        }
    }
}
