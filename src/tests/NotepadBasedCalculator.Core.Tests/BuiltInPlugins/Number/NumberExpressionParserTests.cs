using System.Collections.Generic;
using NotepadBasedCalculator.Api.AbstractSyntaxTree;
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
        public void WordNumberParsing(string input, int output)
        {
            Parser parser = ExportProvider.Import<Parser>();
            IReadOnlyList<IReadOnlyList<Expression>> expressionLines = parser.Parse(input);
            Assert.Equal(output, int.Parse(expressionLines[0][0].ToString()));
        }
    }
}
