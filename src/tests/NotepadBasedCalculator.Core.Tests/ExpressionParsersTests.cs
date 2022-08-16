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
            IData data = ((DataExpression)((GroupExpression)statement.NumericalCalculusExpression).InnerExpression).Data;
            Assert.Equal("[Numeric] (2, 5): '132'", data.ToString());

            parserResult = await parser.ParseAsync(" (  132  ) ");
            Assert.Single(parserResult.Lines[0].Statements);
            statement = (NumericalCalculusStatement)parserResult.Lines[0].Statements[0];
            data = ((DataExpression)((GroupExpression)statement.NumericalCalculusExpression).InnerExpression).Data;
            Assert.Equal("[Numeric] (4, 7): '132'", data.ToString());

            parserResult = await parser.ParseAsync(" (  132 horse ) ");
            Assert.Single(parserResult.Lines[0].Statements);
            statement = (NumericalCalculusStatement)parserResult.Lines[0].Statements[0];
            data = ((DataExpression)((GroupExpression)statement.NumericalCalculusExpression).InnerExpression).Data;
            Assert.Equal("[Numeric] (4, 7): '132'", data.ToString());

            parserResult = await parser.ParseAsync(" ( pigeon 132 horse ) ");
            Assert.Single(parserResult.Lines[0].Statements);
            statement = (NumericalCalculusStatement)parserResult.Lines[0].Statements[0];
            data = ((DataExpression)((GroupExpression)statement.NumericalCalculusExpression).InnerExpression).Data;
            Assert.Equal("[Numeric] (10, 13): '132'", data.ToString());

            parserResult = await parser.ParseAsync(" ( ) ");
            Assert.Empty(parserResult.Lines[0].Statements);

            parserResult = await parser.ParseAsync("1()2");
            Assert.Equal(2, parserResult.Lines[0].Statements.Count);
        }

        [Fact]
        public async Task NumberDataExpression_GroupNested()
        {
            Parser parser = ExportProvider.Import<Parser>();
            ParserResult parserResult = await parser.ParseAsync(" ((132)) ");
            Assert.Single(parserResult.Lines[0].Statements);
            var statement = (NumericalCalculusStatement)parserResult.Lines[0].Statements[0];
            if (statement.NumericalCalculusExpression is not GroupExpression groupExpression)
            {
                Assert.Fail("group expected");
                return;
            }

            if (groupExpression.InnerExpression is not GroupExpression groupExpression2)
            {
                Assert.Fail("group expected");
                return;
            }

            if (groupExpression2.InnerExpression is not DataExpression data)
            {
                Assert.Fail("data expected");
                return;
            }

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
            Assert.Equal("((([Numeric] (1, 4): '123' + (([Numeric] (6, 7): '1' + [Numeric] (9, 10): '2')))) * [Numeric] (15, 17): '-3')", expression.ToString());
        }

        [Fact]
        public async Task NumberDataExpression_ImplicitOperator()
        {
            Parser parser = ExportProvider.Import<Parser>();
            ParserResult parserResult = await parser.ParseAsync("1+(2)(3)");
            Assert.Single(parserResult.Lines[0].Statements);
            var statement = (NumericalCalculusStatement)parserResult.Lines[0].Statements[0];
            var expression = (BinaryOperatorExpression)statement.NumericalCalculusExpression;
            Assert.Equal("([Numeric] (0, 1): '1' + (([Numeric] (3, 4): '2') * ([Numeric] (6, 7): '3')))", expression.ToString());

            /*
             How do we solve (12)3+(1 +2)(3(2))(1 +2)-3 step by step?
                Multiple: 12 * 3 = 36
                Add: 1 + 2 = 3
                Multiple: 3 * 2 = 6
                Multiple: the result of step No. 2 * the result of step No. 3 = 3 * 6 = 18
                Add: 1 + 2 = 3
                Multiple: the result of step No. 4 * the result of step No. 5 = 18 * 3 = 54
                Add: the result of step No. 1 + the result of step No. 6 = 36 + 54 = 90
                Subtract: the result of step No. 7 - 3 = 90 - 3 = 87
             */
            parserResult = await parser.ParseAsync("(12)3+(1 +2)(3(2))(1 +2)-3");
            Assert.Single(parserResult.Lines[0].Statements);
            statement = (NumericalCalculusStatement)parserResult.Lines[0].Statements[0];
            expression = (BinaryOperatorExpression)statement.NumericalCalculusExpression;
            Assert.Equal("((([Numeric] (1, 3): '12') * [Numeric] (4, 5): '3') + ((((([Numeric] (7, 8): '1' + [Numeric] (10, 11): '2')) * (([Numeric] (13, 14): '3' * ([Numeric] (15, 16): '2')))) * (([Numeric] (19, 20): '1' + [Numeric] (22, 23): '2'))) + [Numeric] (24, 26): '-3'))", expression.ToString());
        }
    }
}
