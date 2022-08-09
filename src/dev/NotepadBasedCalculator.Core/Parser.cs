using NotepadBasedCalculator.Api;

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
            return Parse(input, CultureAttribute.English);
        }

        internal IReadOnlyList<IReadOnlyList<Expression>> Parse(string? input, string culture)
        {
            Guard.IsNotNullOrWhiteSpace(culture);

            var expressionLines = new List<List<Expression>>();
            IReadOnlyList<TokenizedTextLine> tokenizedLines = _lexer.Tokenize(input);

            for (int i = 0; i < tokenizedLines.Count; i++)
            {
                var expressions = new List<Expression>();
                TokenizedTextLine tokenizedLine = tokenizedLines[i];

                LinkedToken? nextTokenToParse = tokenizedLine.Tokens;
                while (nextTokenToParse is not null)
                {
                    Expression? expression = ParseExpression(nextTokenToParse, culture);
                    if (expression is not null)
                    {
                        nextTokenToParse = expression.LastToken.Next;
                        expressions.Add(expression);
                    }
                    else
                    {
                        // Ignore the current token. It might be a word that we would simply skip.
                        nextTokenToParse = nextTokenToParse.Next;
                    }
                }

                expressionLines.Add(expressions);
            }

            return expressionLines;
        }

        private Expression? ParseExpression(LinkedToken linkedToken, string culture)
        {
            Expression? expression = null;

            foreach (Lazy<IExpressionParser, ExpressionParserMetadata>? expressionParser in _expressionParsers)
            {
                if (expressionParser.Value.TryParseExpression(linkedToken, culture, out expression)
                    && expression is not null)
                {
                    break;
                }
            }

            return expression;
        }
    }
}
