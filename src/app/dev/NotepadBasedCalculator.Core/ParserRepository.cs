namespace NotepadBasedCalculator.Core
{
    [Export(typeof(IParserRepository))]
    internal sealed class ParserRepository : IParserRepository
    {
        private readonly IEnumerable<Lazy<IDataParser, CultureCodeMetadata>> _dataParsers;
        private readonly IEnumerable<Lazy<IStatementParserAndInterpreter, ParserMetadata>> _statementParsersAndInterpreters;
        private readonly IEnumerable<Lazy<IExpressionParserAndInterpreter, ParserMetadata>> _expressionParsersAndInterpreters;
        private readonly Dictionary<SearchQuery, IEnumerable<IDataParser>> _applicableDataParsers = new();
        private readonly Dictionary<SearchQuery, IEnumerable<IStatementParserAndInterpreter>> _applicableStatementParsersAndInterpreters = new();
        private readonly Dictionary<SearchQuery, IEnumerable<IExpressionParserAndInterpreter>> _applicableExpressionParsersAndInterpreters = new();

        [ImportingConstructor]
        public ParserRepository(
            [ImportMany] IEnumerable<Lazy<IDataParser, CultureCodeMetadata>> dataParsers,
            [ImportMany] IEnumerable<Lazy<IStatementParserAndInterpreter, ParserMetadata>> statementParsersAndInterpreters,
            [ImportMany] IEnumerable<Lazy<IExpressionParserAndInterpreter, ParserMetadata>> expressionParsersAndInterpreters)
        {
            _dataParsers = dataParsers;

            _statementParsersAndInterpreters
                = statementParsersAndInterpreters
                .OrderBy(p => p.Metadata.Order);

            _expressionParsersAndInterpreters
                = expressionParsersAndInterpreters
                .OrderBy(p => p.Metadata.Order);
        }

        public IEnumerable<IDataParser> GetApplicableDataParsers(string culture)
        {
            lock (_applicableDataParsers)
            {
                var key = new SearchQuery(culture);
                if (_applicableDataParsers.TryGetValue(key, out IEnumerable<IDataParser>? parsers) && parsers is not null)
                {
                    return parsers;
                }

                parsers = _dataParsers.Where(
                    p => p.Metadata.CultureCodes.Any(
                        c => CultureHelper.IsCultureApplicable(c, culture)))
                    .Select(p => p.Value);

                _applicableDataParsers[key] = parsers;
                return parsers;
            }
        }

        public IEnumerable<IStatementParserAndInterpreter> GetApplicableStatementParsersAndInterpreters(string culture)
        {
            lock (_applicableStatementParsersAndInterpreters)
            {
                var key = new SearchQuery(culture);
                if (_applicableStatementParsersAndInterpreters.TryGetValue(key, out IEnumerable<IStatementParserAndInterpreter>? parsersAndInterpreters) && parsersAndInterpreters is not null)
                {
                    return parsersAndInterpreters;
                }

                parsersAndInterpreters = _statementParsersAndInterpreters.Where(
                    p => p.Metadata.CultureCodes.Any(
                        c => CultureHelper.IsCultureApplicable(c, culture)))
                    .Select(p => p.Value);

                _applicableStatementParsersAndInterpreters[key] = parsersAndInterpreters;
                return parsersAndInterpreters;
            }
        }

        public IEnumerable<IExpressionParserAndInterpreter> GetApplicableExpressionParsersAndInterpreters(string culture)
        {
            lock (_applicableExpressionParsersAndInterpreters)
            {
                var key = new SearchQuery(culture);
                if (_applicableExpressionParsersAndInterpreters.TryGetValue(key, out IEnumerable<IExpressionParserAndInterpreter>? parsersAndInterpreters) && parsersAndInterpreters is not null)
                {
                    return parsersAndInterpreters;
                }

                parsersAndInterpreters = _expressionParsersAndInterpreters.Where(
                    p => p.Metadata.CultureCodes.Any(
                        c => CultureHelper.IsCultureApplicable(c, culture)))
                    .Select(p => p.Value);

                _applicableExpressionParsersAndInterpreters[key] = parsersAndInterpreters;
                return parsersAndInterpreters;
            }
        }

        public IExpressionParserAndInterpreter GetExpressionParserAndInterpreter(string culture, string expressionParserAndInterpreterName)
        {
            IExpressionParserAndInterpreter parserAndInterpreter
                = _expressionParsersAndInterpreters
                    .Where(
                        p => string.Equals(p.Metadata.Name, expressionParserAndInterpreterName, StringComparison.Ordinal)
                            && p.Metadata.CultureCodes.Any(c => CultureHelper.IsCultureApplicable(c, culture)))
                    .First().Value;

            return parserAndInterpreter;
        }

        private struct SearchQuery : IEquatable<SearchQuery>
        {
            private readonly string _culture;

            internal SearchQuery(string culture)
            {
                _culture = culture;
            }

            public override bool Equals(object? obj)
            {
                return obj is SearchQuery query && Equals(query);
            }

            public bool Equals(SearchQuery other)
            {
                return _culture == other._culture;
            }

            public override int GetHashCode()
            {
                return -498521196 + EqualityComparer<string>.Default.GetHashCode(_culture);
            }
        }
    }
}
