using System.Threading.Tasks;
using NotepadBasedCalculator.Api;
using NotepadBasedCalculator.BuiltInPlugins.Comment;
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
            Assert.Single(parserResult.Lines[0].Expressions);
            Expression expression = parserResult.Lines[0].Expressions[0];
            Assert.IsType<CommentExpression>(expression);
            Assert.Equal("[SymbolOrPunctuation] (5, 6): '/'", expression.FirstToken.ToString());
            Assert.Equal("[Word] (7, 14): 'comment'", expression.LastToken.ToString());

            parserResult = await parser.ParseAsync(" 132 // comment");
            Assert.Single(parserResult.Lines[0].Expressions);
            expression = parserResult.Lines[0].Expressions[0];
            Assert.IsType<CommentExpression>(expression);
            Assert.Equal("[SymbolOrPunctuation] (5, 6): '/'", expression.FirstToken.ToString());
            Assert.Equal("[Word] (8, 15): 'comment'", expression.LastToken.ToString());

            parserResult = await parser.ParseAsync(" 132 // comment // comment 2");
            Assert.Single(parserResult.Lines[0].Expressions);
            expression = parserResult.Lines[0].Expressions[0];
            Assert.IsType<CommentExpression>(expression);
            Assert.Equal("[SymbolOrPunctuation] (5, 6): '/'", expression.FirstToken.ToString());
            Assert.Equal("IntegerData { Type = Numeric, StartInLine = 27, EndInLine = 28, Length = 1, Subtype = Integer, Value = 2 }", expression.LastToken.ToString());

            parserResult = await parser.ParseAsync(" 132 / / comment");
            Assert.Empty(parserResult.Lines[0].Expressions);
        }
    }
}
