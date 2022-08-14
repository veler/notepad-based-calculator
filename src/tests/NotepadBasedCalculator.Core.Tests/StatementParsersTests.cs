using System.Threading.Tasks;
using NotepadBasedCalculator.Api;
using NotepadBasedCalculator.BuiltInPlugins.Statements.Comment;
using NotepadBasedCalculator.BuiltInPlugins.Statements.Header;
using Xunit;

namespace NotepadBasedCalculator.Core.Tests
{
    public sealed class StatementParsersTests : MefBaseTest
    {
        [Fact]
        public async Task CommentStatement()
        {
            Parser parser = ExportProvider.Import<Parser>();
            ParserResult parserResult = await parser.ParseAsync(" 132 //comment");
            Assert.Single(parserResult.Lines[0].Statements);
            Statement statement = parserResult.Lines[0].Statements[0];
            Assert.IsType<CommentStatement>(statement);
            Assert.Equal("[SymbolOrPunctuation] (5, 6): '/'", statement.FirstToken.ToString());
            Assert.Equal("[Word] (7, 14): 'comment'", statement.LastToken.ToString());

            parserResult = await parser.ParseAsync(" 132 // comment");
            Assert.Single(parserResult.Lines[0].Statements);
            statement = parserResult.Lines[0].Statements[0];
            Assert.IsType<CommentStatement>(statement);
            Assert.Equal("[SymbolOrPunctuation] (5, 6): '/'", statement.FirstToken.ToString());
            Assert.Equal("[Word] (8, 15): 'comment'", statement.LastToken.ToString());

            parserResult = await parser.ParseAsync(" 132 // comment // comment 2");
            Assert.Single(parserResult.Lines[0].Statements);
            statement = parserResult.Lines[0].Statements[0];
            Assert.IsType<CommentStatement>(statement);
            Assert.Equal("[SymbolOrPunctuation] (5, 6): '/'", statement.FirstToken.ToString());
            Assert.Equal("IntegerData { Type = Numeric, StartInLine = 27, EndInLine = 28, Length = 1, Subtype = Integer, Value = 2 }", statement.LastToken.ToString());

            parserResult = await parser.ParseAsync(" 132 / / comment");
            Assert.Empty(parserResult.Lines[0].Statements);
        }

        [Fact]
        public async Task HeaderStatement()
        {
            Parser parser = ExportProvider.Import<Parser>();
            ParserResult parserResult = await parser.ParseAsync("#Header");
            Assert.Single(parserResult.Lines[0].Statements);
            Statement statement = parserResult.Lines[0].Statements[0];
            Assert.IsType<HeaderStatement>(statement);
            Assert.Equal("[SymbolOrPunctuation] (0, 1): '#'", statement.FirstToken.ToString());
            Assert.Equal("[Word] (1, 7): 'Header'", statement.LastToken.ToString());

            parserResult = await parser.ParseAsync(" # Header");
            Assert.Single(parserResult.Lines[0].Statements);
            statement = parserResult.Lines[0].Statements[0];
            Assert.IsType<HeaderStatement>(statement);
            Assert.Equal("[Whitespace] (0, 1): ' '", statement.FirstToken.ToString());
            Assert.Equal("[Word] (3, 9): 'Header'", statement.LastToken.ToString());

            parserResult = await parser.ParseAsync(" ### Header");
            Assert.Single(parserResult.Lines[0].Statements);
            statement = parserResult.Lines[0].Statements[0];
            Assert.IsType<HeaderStatement>(statement);
            Assert.Equal("[Whitespace] (0, 1): ' '", statement.FirstToken.ToString());
            Assert.Equal("[Word] (5, 11): 'Header'", statement.LastToken.ToString());

            parserResult = await parser.ParseAsync(" 132 # Header");
            Assert.Empty(parserResult.Lines[0].Statements);
        }
    }
}
