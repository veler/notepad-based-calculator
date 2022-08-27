using System.Collections.Generic;
using System.Threading.Tasks;
using NotepadBasedCalculator.Api;
using NotepadBasedCalculator.BuiltInPlugins.StatementParsersAndInterpreters.Comment;
using NotepadBasedCalculator.BuiltInPlugins.StatementParsersAndInterpreters.Header;
using NotepadBasedCalculator.BuiltInPlugins.StatementParsersAndInterpreters.NumericalExpression;
using Xunit;

namespace NotepadBasedCalculator.Core.Tests
{
    public class ParserTests : MefBaseTest
    {
        private readonly ParserAndInterpreter _parserAndInterpreter;
        private readonly TextDocument _textDocument;

        public ParserTests()
        {
            ParserAndInterpreterFactory parserAndInterpreterFactory = ExportProvider.Import<ParserAndInterpreterFactory>();
            _textDocument = new TextDocument();
            _parserAndInterpreter = parserAndInterpreterFactory.CreateInstance(SupportedCultures.English, _textDocument);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _parserAndInterpreter.Dispose();
        }

        [Fact]
        public async Task SimpleAsync()
        {
            _textDocument.Text =
@" # This is a header. 123. By the way I have 456% chance to get it to work.


I got -123 dollars in my pocket. // this is a comment.";

            IReadOnlyList<ParserAndInterpreterResultLine> lineResults = await _parserAndInterpreter.WaitAsync();
            Assert.Equal(4, lineResults.Count);
            Assert.Equal(1, lineResults[0].StatementsAndData.Count);
            Assert.Equal(0, lineResults[1].StatementsAndData.Count);
            Assert.Equal(0, lineResults[2].StatementsAndData.Count);
            Assert.Equal(2, lineResults[3].StatementsAndData.Count);

            Assert.IsType<HeaderStatement>(lineResults[0].StatementsAndData[0].ParsedStatement);
            Assert.IsType<NumericalCalculusStatement>(lineResults[3].StatementsAndData[0].ParsedStatement);
            Assert.IsType<CommentStatement>(lineResults[3].StatementsAndData[1].ParsedStatement);
        }
    }
}
