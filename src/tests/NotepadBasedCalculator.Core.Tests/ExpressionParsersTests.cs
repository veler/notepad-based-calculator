using System.Threading.Tasks;
using NotepadBasedCalculator.Api;
using NotepadBasedCalculator.BuiltInPlugins.Statements.NumericalCalculus;
using Xunit;

namespace NotepadBasedCalculator.Core.Tests
{
    public sealed class ExpressionParsersTests : MefBaseTest
    {
        [Fact]
        public async Task NumberDataExpression_Integer()
        {
            Parser parser = ExportProvider.Import<Parser>();
            ParserResult parserResult = await parser.ParseAsync(" 132 ");
            Assert.Single(parserResult.Lines[0].Statements);
            var statement = (NumericalCalculusStatement)parserResult.Lines[0].Statements[0];
            IData data = ((DataExpression)statement.NumericalCalculusExpression).Data;
            Assert.Equal("IntegerData { Type = Numeric, StartInLine = 1, EndInLine = 4, Length = 3, Subtype = Integer, Value = 132 }", data.ToString());
        }

        [Fact]
        public async Task NumberDataExpression_Group()
        {
            Parser parser = ExportProvider.Import<Parser>();
            ParserResult parserResult = await parser.ParseAsync(" (132) ");
            Assert.Single(parserResult.Lines[0].Statements);
            var statement = (NumericalCalculusStatement)parserResult.Lines[0].Statements[0];
            IData data = ((DataExpression)statement.NumericalCalculusExpression).Data;
            Assert.Equal("IntegerData { Type = Numeric, StartInLine = 2, EndInLine = 5, Length = 3, Subtype = Integer, Value = 132 }", data.ToString());

            parserResult = await parser.ParseAsync(" (  132  ) ");
            Assert.Single(parserResult.Lines[0].Statements);
            statement = (NumericalCalculusStatement)parserResult.Lines[0].Statements[0];
            data = ((DataExpression)statement.NumericalCalculusExpression).Data;
            Assert.Equal("IntegerData { Type = Numeric, StartInLine = 4, EndInLine = 7, Length = 3, Subtype = Integer, Value = 132 }", data.ToString());

            parserResult = await parser.ParseAsync(" (  132 horse ) ");
            Assert.Single(parserResult.Lines[0].Statements);
            statement = (NumericalCalculusStatement)parserResult.Lines[0].Statements[0];
            data = ((DataExpression)statement.NumericalCalculusExpression).Data;
            Assert.Equal("IntegerData { Type = Numeric, StartInLine = 4, EndInLine = 7, Length = 3, Subtype = Integer, Value = 132 }", data.ToString());
        }

        [Fact]
        public async Task NumberDataExpression_GroupNested()
        {
            Parser parser = ExportProvider.Import<Parser>();
            ParserResult parserResult = await parser.ParseAsync(" ((132)) ");
            Assert.Single(parserResult.Lines[0].Statements);
            var statement = (NumericalCalculusStatement)parserResult.Lines[0].Statements[0];
            IData data = ((DataExpression)statement.NumericalCalculusExpression).Data;
            Assert.Equal("IntegerData { Type = Numeric, StartInLine = 3, EndInLine = 6, Length = 3, Subtype = Integer, Value = 132 }", data.ToString());
        }
    }
}
