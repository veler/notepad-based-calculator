using NotepadBasedCalculator.Api;
using NotepadBasedCalculator.Api.AbstractSyntaxTree;

namespace NotepadBasedCalculator.Core
{
    [Export]
    internal sealed class Parser
    {
        private readonly IEnumerable<Lazy<IExpressionParser, ExpressionParserMetadata>> _expressionParsers;
        private readonly Lexer _lexer = new();

        [ImportingConstructor]
        public Parser([ImportMany] IEnumerable<Lazy<IExpressionParser, ExpressionParserMetadata>> expressionParsers)
        {
            _expressionParsers
                = expressionParsers
                .OrderBy(p => p.Metadata.Order);
        }

        internal IReadOnlyList<IReadOnlyList<Expression>> Parse(string? input)
        {
            var expressionLines = new List<List<Expression>>();
            int tokenEndIndexWithCarriageReturn = 0;
            LinkedToken? lineTokens = _lexer.GetLineTokens(input, startIndex: 0);

            while (lineTokens is not null)
            {
                var expressions = new List<Expression>();

                while (lineTokens is not null)
                {
                    Expression? expression = ParseExpression(lineTokens);
                    if (expression is not null)
                    {
                        tokenEndIndexWithCarriageReturn = expression.LastToken.TokenEndIndexWithCarriageReturn;
                        lineTokens = expression.LastToken.Next;
                        expressions.Add(expression);
                    }
                    else
                    {
                        // Ignore the current token. It might be a word that we would simply skip.
                        tokenEndIndexWithCarriageReturn = lineTokens.TokenEndIndexWithCarriageReturn;
                        lineTokens = lineTokens.Next;
                    }
                }

                lineTokens = _lexer.GetLineTokens(input, startIndex: tokenEndIndexWithCarriageReturn);
                expressionLines.Add(expressions);
            }

            return expressionLines;
        }

        private Expression? ParseExpression(LinkedToken linkedToken)
        {
            Expression? expression = null;

            foreach (Lazy<IExpressionParser, ExpressionParserMetadata>? expressionParser in _expressionParsers)
            {
                if (expressionParser.Value.TryParseExpression(linkedToken, out expression)
                    && expression is not null)
                {
                    break;
                }
            }

            return expression;
        }
    }
}
