using Microsoft.Recognizers.Text;

namespace NotepadBasedCalculator.Core
{
    [Export]
    internal sealed class Parser
    {
        private readonly IEnumerable<Lazy<IDataParser, DataParserMetadata>> _dataParsers;
        private readonly IEnumerable<Lazy<IExpressionParser, ExpressionParserMetadata>> _expressionParsers;

        [ImportingConstructor]
        public Parser(
            [ImportMany] IEnumerable<Lazy<IDataParser, DataParserMetadata>> dataParsers,
            [ImportMany] IEnumerable<Lazy<IExpressionParser, ExpressionParserMetadata>> expressionParsers)
        {
            _dataParsers = dataParsers;
            _expressionParsers
                = expressionParsers
                .OrderBy(p => p.Metadata.Order);
        }

        internal Task<ParserResult> ParseAsync(string? input)
        {
            return ParseAsync(input, SupportedCultures.English);
        }

        internal async Task<ParserResult> ParseAsync(string? input, string culture)
        {
            Guard.IsNotNullOrWhiteSpace(culture);
            culture = Culture.MapToNearestLanguage(culture);

            IReadOnlyList<Lazy<IDataParser, DataParserMetadata>> applicableDataParsers
                = GetApplicableDataParsers(culture);

            var resultLines = new List<ParserResultLine>();
            IReadOnlyList<TokenizedTextLine> tokenizedLines = Lexer.Tokenize(input);

            for (int i = 0; i < tokenizedLines.Count; i++)
            {
                TokenizedTextLine tokenizedLine = tokenizedLines[i];

                IReadOnlyList<IData> parsedData = await ParseDataAsync(culture, applicableDataParsers, tokenizedLine);

                tokenizedLine = Lexer.TokenizeLine(tokenizedLine.Start, tokenizedLine.LineTextIncludingLineBreak, parsedData);

                var expressions = new List<Expression>();
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

                resultLines.Add(new ParserResultLine(tokenizedLine, parsedData, expressions));
            }

            return new ParserResult(resultLines);
        }

        private Expression? ParseExpression(LinkedToken linkedToken, string culture)
        {
            Expression? expression = null;

            foreach (Lazy<IExpressionParser, ExpressionParserMetadata>? expressionParser in _expressionParsers)
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

        private static async Task<IReadOnlyList<IData>> ParseDataAsync(string culture, IReadOnlyList<Lazy<IDataParser, DataParserMetadata>> dataParsers, TokenizedTextLine tokenizedLine)
        {
            var rawDataBag = new List<IData>();
            var nonOverlappingData = new List<IData>();
            var tasks = new List<Task>();

            for (int i = 0; i < dataParsers.Count; i++)
            {
                IDataParser parser = dataParsers[i].Value;
                tasks.Add(
                    Task.Run(
                        () =>
                        {
                            IReadOnlyList<IData>? results = parser.Parse(culture, tokenizedLine);
                            if (results is not null)
                            {
                                lock (rawDataBag)
                                {
                                    rawDataBag.AddRange(results);
                                }
                            }
                        }));
            }

            await Task.WhenAll(tasks);

            // For each data we parsed, find whether the data is overlapped by another one. If not, then we keep it.
            for (int i = 0; i < rawDataBag.Count; i++)
            {
                IData currentData = rawDataBag[i];
                if (currentData is not null
                    && !IsDataOverlapped(rawDataBag, currentData))
                {
                    nonOverlappingData.Add(currentData);
                }
            }

            // Sort the non-overlapping items.
            nonOverlappingData.Sort();

            return nonOverlappingData;
        }

        private static bool IsDataOverlapped(IReadOnlyList<IData> allData, IData currentData)
        {
            for (int i = 0; i < allData.Count; i++)
            {
                IData data = allData[i];
                if (data is not null
                    && currentData != data
                    && data.StartInLine <= currentData.StartInLine
                    && data.EndInLine >= currentData.EndInLine)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
