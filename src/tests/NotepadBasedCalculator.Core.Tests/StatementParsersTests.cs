using System.Threading.Tasks;
using NotepadBasedCalculator.Api;
using NotepadBasedCalculator.BuiltInPlugins.Statements.Comment;
using NotepadBasedCalculator.BuiltInPlugins.Statements.Header;
using NotepadBasedCalculator.BuiltInPlugins.Statements.NumericalCalculus;
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
            Assert.Equal(2, parserResult.Lines[0].Statements.Count);
            Statement statement = parserResult.Lines[0].Statements[1];
            Assert.IsType<CommentStatement>(statement);
            Assert.Equal("[comment_operators] (5, 7): '//'", statement.FirstToken.ToString());
            Assert.Equal("[Word] (7, 14): 'comment'", statement.LastToken.ToString());

            parserResult = await parser.ParseAsync(" 132 // comment");
            Assert.Equal(2, parserResult.Lines[0].Statements.Count);
            statement = parserResult.Lines[0].Statements[1];
            Assert.IsType<CommentStatement>(statement);
            Assert.Equal("[comment_operators] (5, 7): '//'", statement.FirstToken.ToString());
            Assert.Equal("[Word] (8, 15): 'comment'", statement.LastToken.ToString());

            parserResult = await parser.ParseAsync(" 132 // comment // comment 2");
            Assert.Equal(2, parserResult.Lines[0].Statements.Count);
            statement = parserResult.Lines[0].Statements[1];
            Assert.IsType<CommentStatement>(statement);
            Assert.Equal("[comment_operators] (5, 7): '//'", statement.FirstToken.ToString());
            Assert.Equal("[Numeric] (27, 28): '2'", statement.LastToken.ToString());

            parserResult = await parser.ParseAsync(" 132 / / comment");
            Assert.Single(parserResult.Lines[0].Statements);
            statement = parserResult.Lines[0].Statements[0];
            Assert.IsType<NumericalCalculusStatement>(statement);
        }

        [Fact]
        public async Task HeaderStatement()
        {
            Parser parser = ExportProvider.Import<Parser>();
            ParserResult parserResult = await parser.ParseAsync("#Header");
            Assert.Single(parserResult.Lines[0].Statements);
            Statement statement = parserResult.Lines[0].Statements[0];
            Assert.IsType<HeaderStatement>(statement);
            Assert.Equal("[header_operators] (0, 1): '#'", statement.FirstToken.ToString());
            Assert.Equal("[Word] (1, 7): 'Header'", statement.LastToken.ToString());

            parserResult = await parser.ParseAsync(" # Header");
            Assert.Single(parserResult.Lines[0].Statements);
            statement = parserResult.Lines[0].Statements[0];
            Assert.IsType<HeaderStatement>(statement);
            Assert.Equal("[header_operators] (1, 2): '#'", statement.FirstToken.ToString());
            Assert.Equal("[Word] (3, 9): 'Header'", statement.LastToken.ToString());

            parserResult = await parser.ParseAsync(" ### Header");
            Assert.Single(parserResult.Lines[0].Statements);
            statement = parserResult.Lines[0].Statements[0];
            Assert.IsType<HeaderStatement>(statement);
            Assert.Equal("[header_operators] (1, 4): '###'", statement.FirstToken.ToString());
            Assert.Equal("[Word] (5, 11): 'Header'", statement.LastToken.ToString());

            parserResult = await parser.ParseAsync(" 132 # Header");
            Assert.Single(parserResult.Lines[0].Statements);
            statement = parserResult.Lines[0].Statements[0];
            Assert.IsType<NumericalCalculusStatement>(statement);
        }

        [Fact]
        public async Task ConditionStatement()
        {
            string input = "if one hundred thousand dollars of income + (30% tax / two people) > 150k then test";

            Parser parser = ExportProvider.Import<Parser>();
            ParserResult parserResult = await parser.ParseAsync(input);
            Assert.Single(parserResult.Lines[0].Statements);
            Statement statement = parserResult.Lines[0].Statements[0];
            //Assert.IsType<HeaderStatement>(statement);
            //Assert.Equal("[SymbolOrPunctuation] (0, 1): '#'", statement.FirstToken.ToString());
            //Assert.Equal("[Word] (1, 7): 'Header'", statement.LastToken.ToString());

            input = "if one > 2 or 3 <= 4 <= 2 then test";
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
