using Microsoft.Recognizers.Text;

namespace NotepadBasedCalculator.Core
{
    [Export]
    internal sealed class Parser
    {
        private readonly IEnumerable<Lazy<IDataParser, DataParserMetadata>> _dataParsers;
        private readonly IEnumerable<Lazy<IStatementParser, ExpressionParserMetadata>> _expressionParsers;
        private readonly Lexer _lexer = new();

        [ImportingConstructor]
        public Parser(
            [ImportMany] IEnumerable<Lazy<IDataParser, DataParserMetadata>> dataParsers,
            [ImportMany] IEnumerable<Lazy<IStatementParser, ExpressionParserMetadata>> expressionParsers)
        {
            _dataParsers = dataParsers;
            _expressionParsers
                = expressionParsers
                .OrderBy(p => p.Metadata.Order);
        }

        internal Task<IReadOnlyList<IReadOnlyList<Expression>>> ParseAsync(string? input)
        {
            return ParseAsync(input, SupportedCultures.English);
        }

        internal async Task<IReadOnlyList<IReadOnlyList<Expression>>> ParseAsync(string? input, string culture)
        {
            Guard.IsNotNullOrWhiteSpace(culture);
            culture = Culture.MapToNearestLanguage(culture);

            IReadOnlyList<Lazy<IDataParser, DataParserMetadata>> applicableDataParsers
                = GetApplicableDataParsers(culture);

            var expressionLines = new List<List<Expression>>();
            IReadOnlyList<TokenizedTextLine> tokenizedLines = _lexer.Tokenize(input);

            for (int i = 0; i < tokenizedLines.Count; i++)
            {
                TokenizedTextLine tokenizedLine = tokenizedLines[i];

                await ParseDataAsync(culture, applicableDataParsers, tokenizedLine);

                //var expressions = new List<Expression>();
                //LinkedToken? nextTokenToParse = tokenizedLine.Tokens;
                //while (nextTokenToParse is not null)
                //{
                //    Expression? expression = ParseExpression(nextTokenToParse, culture);
                //    if (expression is not null)
                //    {
                //        nextTokenToParse = expression.LastToken.Next;
                //        expressions.Add(expression);
                //    }
                //    else
                //    {
                //        // Ignore the current token. It might be a word that we would simply skip.
                //        nextTokenToParse = nextTokenToParse.Next;
                //    }
                //}

                //expressionLines.Add(expressions);
            }

            return expressionLines;
        }

        private Expression? ParseExpression(LinkedToken linkedToken, string culture)
        {
            Expression? expression = null;

            foreach (Lazy<IStatementParser, ExpressionParserMetadata>? expressionParser in _expressionParsers)
            {
                if (expressionParser.Value.TryParseExpression(culture, linkedToken, out expression)
                    && expression is not null)
                {
                    break;
                }
            }

            return expression;
        }

        private IReadOnlyList<Lazy<IDataParser, DataParserMetadata>> GetApplicableDataParsers(string culture)
        {
            return _dataParsers.Where(
                p => p.Metadata.CultureCodes.Any(
                    c => c == SupportedCultures.Any
                        || string.Equals(c, culture, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        private async Task ParseDataAsync(string culture, IReadOnlyList<Lazy<IDataParser, DataParserMetadata>> dataParsers, TokenizedTextLine tokenizedLine)
        {
            var tasks = new List<Task<IReadOnlyList<IData>?>>();
            for (int i = 0; i < dataParsers.Count; i++)
            {
                IDataParser parser = dataParsers[i].Value;
                tasks.Add(
                    Task.Run(
                        () => parser.Parse(culture, tokenizedLine)));
            }

            await Task.WhenAll(tasks);

            IEnumerable<IData> data = tasks.SelectMany(t => t.Result);
            IReadOnlyList<IData> dataWithNoOverlap = data.Intersect(data, new DataOverlapComparer()).ToList();
        }

        private class DataOverlapComparer : IEqualityComparer<IData>
        {
            public bool Equals(IData x, IData y)
            {
                return x.StartInLine < y.EndInLine && y.StartInLine < y.EndInLine;
            }

            public int GetHashCode(IData obj)
            {
                return 0; // On purpose so Equals will be called.
            }
        }
    }
}
