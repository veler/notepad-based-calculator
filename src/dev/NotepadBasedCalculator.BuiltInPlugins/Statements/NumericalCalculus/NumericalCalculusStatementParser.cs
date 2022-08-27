using NotepadBasedCalculator.BuiltInPlugins.StatementParsersAndInterpreters.NumericalExpression;

namespace NotepadBasedCalculator.BuiltInPlugins.Statements.NumericalCalculus
{
    [Export(typeof(IStatementParser))]
    [Culture(SupportedCultures.Any)]
    [Order(int.MaxValue - 1)]
    internal sealed class NumericalCalculusStatementParser : ParserBase, IStatementParser
    {
        public bool TryParseStatement(string culture, LinkedToken currentToken, out Statement? statement)
        {
            Expression? expression = ParseExpression(PredefinedExpressionParserNames.NumericalExpression, culture, currentToken, out _);
            if (expression is not null)
            {
                statement = new NumericalCalculusStatement(expression.FirstToken, expression.LastToken, expression);
                return true;
            }

            statement = null;
            return false;
        }
    }
}
