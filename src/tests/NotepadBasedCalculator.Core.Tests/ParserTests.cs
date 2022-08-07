using System.Collections.Generic;
using NotepadBasedCalculator.Api.AbstractSyntaxTree;
using Xunit;

namespace NotepadBasedCalculator.Core.Tests
{
    public class ParserTests : MefBaseTest
    {
        [Fact]
        public void Simple()
        {
            string input =
@" # This is a header


I got -123 dollars in my pocket. // this is a comment.";

            Parser parser = ExportProvider.Import<Parser>();
            IReadOnlyList<IReadOnlyList<Expression>> expressionLines = parser.Parse(input);
            Assert.Equal(4, expressionLines.Count);
            Assert.Equal(1, expressionLines[0].Count);
            Assert.Equal(0, expressionLines[1].Count);
            Assert.Equal(0, expressionLines[2].Count);
            Assert.Equal(2, expressionLines[3].Count);
        }
    }
}
