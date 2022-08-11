using System.Threading.Tasks;
using Xunit;

namespace NotepadBasedCalculator.Core.Tests.BuiltInPlugins.Number
{
    public sealed class NumberExpressionParserTests : MefBaseTest
    {
        [Theory]
        [InlineData("thirty five thousand", 35000)]
        [InlineData("forty three thousand", 43000)]
        [InlineData("one hundred and fifty thousand dollars", 150000)]
        [InlineData("nine hundred and seventy four thousand", 974000)]
        public async Task WordNumberParsingAsync(string input, int output)
        {
            Parser parser = ExportProvider.Import<Parser>();
            ParserResult parserResult = await parser.ParseAsync(input);
            Assert.Equal(output, int.Parse(parserResult.ToString()));
        }
    }
}
