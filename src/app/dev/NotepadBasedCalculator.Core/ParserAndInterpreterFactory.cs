namespace NotepadBasedCalculator.Core
{
    [Export]
    internal sealed class ParserAndInterpreterFactory
    {
        private readonly ILogger _logger;
        private readonly ILexer _lexer;
        private readonly IParserRepository _parserRepository;

        [ImportingConstructor]
        public ParserAndInterpreterFactory(
            ILogger logger,
            ILexer lexer,
            IParserRepository parserRepository)
        {
            _logger = logger;
            _lexer = lexer;
            _parserRepository = parserRepository;
        }

        internal ParserAndInterpreter CreateInstance(string culture, TextDocument textDocument)
        {
            return new ParserAndInterpreter(
                culture,
                _logger,
                _lexer,
                _parserRepository,
                textDocument);
        }
    }
}
