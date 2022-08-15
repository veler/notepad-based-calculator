using System.Linq.Expressions;
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
            Assert.Equal("[Numeric] (1, 4): '132'", data.ToString());
        }

        [Fact]
        public async Task NumberDataExpression_Group()
        {
            Parser parser = ExportProvider.Import<Parser>();
            ParserResult parserResult = await parser.ParseAsync(" (132) ");
            Assert.Single(parserResult.Lines[0].Statements);
            var statement = (NumericalCalculusStatement)parserResult.Lines[0].Statements[0];
            IData data = ((DataExpression)statement.NumericalCalculusExpression).Data;
            Assert.Equal("[Numeric] (2, 5): '132'", data.ToString());

            parserResult = await parser.ParseAsync(" (  132  ) ");
            Assert.Single(parserResult.Lines[0].Statements);
            statement = (NumericalCalculusStatement)parserResult.Lines[0].Statements[0];
            data = ((DataExpression)statement.NumericalCalculusExpression).Data;
            Assert.Equal("[Numeric] (4, 7): '132'", data.ToString());

            parserResult = await parser.ParseAsync(" (  132 horse ) ");
            Assert.Single(parserResult.Lines[0].Statements);
            statement = (NumericalCalculusStatement)parserResult.Lines[0].Statements[0];
            data = ((DataExpression)statement.NumericalCalculusExpression).Data;
            Assert.Equal("[Numeric] (4, 7): '132'", data.ToString());

            parserResult = await parser.ParseAsync(" ( pigeon 132 horse ) ");
            Assert.Single(parserResult.Lines[0].Statements);
            statement = (NumericalCalculusStatement)parserResult.Lines[0].Statements[0];
            data = ((DataExpression)statement.NumericalCalculusExpression).Data;
            Assert.Equal("[Numeric] (10, 13): '132'", data.ToString());
        }

        [Fact]
        public async Task NumberDataExpression_GroupNested()
        {
            Parser parser = ExportProvider.Import<Parser>();
            ParserResult parserResult = await parser.ParseAsync(" ((132)) ");
            Assert.Single(parserResult.Lines[0].Statements);
            var statement = (NumericalCalculusStatement)parserResult.Lines[0].Statements[0];
            IData data = ((DataExpression)statement.NumericalCalculusExpression).Data;
            Assert.Equal("[Numeric] (3, 6): '132'", data.ToString());
        }

        [Fact]
        public async Task NumberDataExpression_MultiplicationAndDivision()
        {
            Parser parser = ExportProvider.Import<Parser>();
            ParserResult parserResult = await parser.ParseAsync(" 123 horses * 2 trucks x 5 people divided by a farm with 8 fields ");
            Assert.Single(parserResult.Lines[0].Statements);
            var statement = (NumericalCalculusStatement)parserResult.Lines[0].Statements[0];
            var expression = (BinaryOperatorExpression)statement.NumericalCalculusExpression;
            Assert.Equal("((([Numeric] (1, 4): '123' * [Numeric] (14, 15): '2') * [Numeric] (25, 26): '5') / [Numeric] (57, 58): '8')", expression.ToString());
        }

        [Fact]
        public async Task NumberDataExpression_AdditionAndSubstraction()
        {
            Parser parser = ExportProvider.Import<Parser>();
            ParserResult parserResult = await parser.ParseAsync(" I got 123 dogs plus 1 cat and two goldfish, minus 1 death ");
            Assert.Equal(2, parserResult.Lines[0].Statements.Count);
            var statement = (NumericalCalculusStatement)parserResult.Lines[0].Statements[0];
            var expression1 = (BinaryOperatorExpression)statement.NumericalCalculusExpression;
            statement = (NumericalCalculusStatement)parserResult.Lines[0].Statements[1];
            var expression2 = (DataExpression)statement.NumericalCalculusExpression;
            Assert.Equal("(([Numeric] (7, 10): '123' + [Numeric] (21, 22): '1') + [Numeric] (31, 34): 'two')", expression1.ToString());
            Assert.Equal("[Numeric] (45, 52): 'minus 1'", expression2.ToString());
        }

        [Fact]
        public async Task NumberDataExpression_ComplexScenario()
        {
            Parser parser = ExportProvider.Import<Parser>();
            ParserResult parserResult = await parser.ParseAsync("(123+(1 +2)) * -3");
            Assert.Single(parserResult.Lines[0].Statements);
            var statement = (NumericalCalculusStatement)parserResult.Lines[0].Statements[0];
            var expression = (BinaryOperatorExpression)statement.NumericalCalculusExpression;
            Assert.Equal("(([Numeric] (1, 4): '123' + ([Numeric] (6, 7): '1' + [Numeric] (9, 10): '2')) * [Numeric] (15, 17): '-3')", expression.ToString());
        }
    }
}
