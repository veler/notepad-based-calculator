namespace NotepadBasedCalculator.Core
{
    [Export(typeof(IParserRepository))]
    internal sealed class ParserRepository : IParserRepository
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IEnumerable<Lazy<IDataParser, CultureCodeMetadata>> _dataParsers;
        private readonly IEnumerable<Lazy<IExpressionParser, ParserMetadata>> _expressionParsers;
        private readonly IEnumerable<Lazy<IStatementParser, ParserMetadata>> _statementParsers;
        private readonly Dictionary<SearchQuery, IEnumerable<IDataParser>> _applicableDataParsers = new();
        private readonly Dictionary<SearchQuery, IEnumerable<IExpressionParser>> _applicableExpressionParsers = new();
        private readonly Dictionary<SearchQuery, IEnumerable<IStatementParser>> _applicableStatementParsers = new();

        [ImportingConstructor]
        public ParserRepository(
            IServiceProvider serviceProvider,
            [ImportMany] IEnumerable<Lazy<IDataParser, CultureCodeMetadata>> dataParsers,
            [ImportMany] IEnumerable<Lazy<IExpressionParser, ParserMetadata>> expressionParsers,
            [ImportMany] IEnumerable<Lazy<IStatementParser, ParserMetadata>> statementParsers)
        {
            _serviceProvider = serviceProvider;
            _dataParsers = dataParsers;

            _expressionParsers
                = expressionParsers
                .OrderBy(p => p.Metadata.Order);

            _statementParsers
                = statementParsers
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
                        c => IsCultureApplicable(c, culture)))
                    .Select(p =>
                    {
                        if (p.Value is ParserBase parserBase)
                        {
                            parserBase.ServiceProvider = _serviceProvider;
                        }
                        return p.Value;
                    });

                _applicableDataParsers[key] = parsers;
                return parsers;
            }
        }

        public IEnumerable<IExpressionParser> GetApplicableExpressionParsers(string culture)
        {
            lock (_applicableExpressionParsers)
            {
                var key = new SearchQuery(culture);
                if (_applicableExpressionParsers.TryGetValue(key, out IEnumerable<IExpressionParser>? parsers) && parsers is not null)
                {
                    return parsers;
                }

                parsers = _expressionParsers.Where(
                    p => p.Metadata.CultureCodes.Any(
                        c => IsCultureApplicable(c, culture)))
                    .Select(p =>
                    {
                        if (p.Value is ParserBase parserBase)
                        {
                            parserBase.ServiceProvider = _serviceProvider;
                        }
                        return p.Value;
                    });

                _applicableExpressionParsers[key] = parsers;
                return parsers;
            }
        }

        public IEnumerable<IStatementParser> GetApplicableStatementParsers(string culture)
        {
            lock (_applicableStatementParsers)
            {
                var key = new SearchQuery(culture);
                if (_applicableStatementParsers.TryGetValue(key, out IEnumerable<IStatementParser>? parsers) && parsers is not null)
                {
                    return parsers;
                }

                parsers = _statementParsers.Where(
                    p => p.Metadata.CultureCodes.Any(
                        c => IsCultureApplicable(c, culture)))
                    .Select(p =>
                    {
                        if (p.Value is ParserBase parserBase)
                        {
                            parserBase.ServiceProvider = _serviceProvider;
                        }
                        return p.Value;
                    });

                _applicableStatementParsers[key] = parsers;
                return parsers;
            }
        }

        private static bool IsCultureApplicable(string culture, string targetCulture)
        {
            return culture == SupportedCultures.Any || string.Equals(culture, targetCulture, StringComparison.OrdinalIgnoreCase);
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
