using System.Threading.Tasks;
using Xunit;

namespace NotepadBasedCalculator.Core.Tests
{
    public class ParserTests : MefBaseTest
    {
        [Fact]
        public async Task SimpleAsync()
        {
            string input =
@" # This is a header. 123. By the way I have 456% chance to get it to work.


I got -123 dollars in my pocket. // this is a comment.";

            Parser parser = ExportProvider.Import<Parser>();
            ParserResult parserResult = await parser.ParseAsync(input);
            Assert.Fail("");
            //Assert.Equal(4, expressionLines.Count);
            //Assert.Equal(1, expressionLines[0].Count);
            //Assert.Equal(0, expressionLines[1].Count);
            //Assert.Equal(0, expressionLines[2].Count);
            //Assert.Equal(2, expressionLines[3].Count);
        }
    }
}
