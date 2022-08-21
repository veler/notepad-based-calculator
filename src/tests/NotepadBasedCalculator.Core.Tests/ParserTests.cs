using System.Threading.Tasks;
using NotepadBasedCalculator.BuiltInPlugins.Statements.Comment;
using NotepadBasedCalculator.BuiltInPlugins.Statements.Header;
using NotepadBasedCalculator.BuiltInPlugins.Statements.NumericalCalculus;
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
            Assert.Equal(4, parserResult.Lines.Count);
            Assert.Equal(1, parserResult.Lines[0].Statements.Count);
            Assert.Equal(0, parserResult.Lines[1].Statements.Count);
            Assert.Equal(0, parserResult.Lines[2].Statements.Count);
            Assert.Equal(2, parserResult.Lines[3].Statements.Count);

            Assert.IsType(typeof(HeaderStatement), parserResult.Lines[0].Statements[0]);
            Assert.IsType(typeof(NumericalCalculusStatement), parserResult.Lines[3].Statements[0]);
            Assert.IsType(typeof(CommentStatement), parserResult.Lines[3].Statements[1]);
        }
    }
}
