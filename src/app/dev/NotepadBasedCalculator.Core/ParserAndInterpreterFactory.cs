namespace NotepadBasedCalculator.Core
{
    [Export]
    internal sealed class ParserAndInterpreterFactory
    {
        private readonly ILogger _logger;
        private readonly ILexer _lexer;
        private readonly IParserAndInterpretersRepository _parserRepository;
        private readonly IArithmeticAndRelationOperationService _arithmeticAndRelationOperationService;

        [ImportingConstructor]
        public ParserAndInterpreterFactory(
            ILogger logger,
            ILexer lexer,
            IParserAndInterpretersRepository parserRepository,
            IArithmeticAndRelationOperationService arithmeticAndRelationOperationService)
        {
            _logger = logger;
            _lexer = lexer;
            _parserRepository = parserRepository;
            _arithmeticAndRelationOperationService = arithmeticAndRelationOperationService;
        }

        internal ParserAndInterpreter CreateInstance(string culture, TextDocument textDocument)
        {
            return new ParserAndInterpreter(
                culture,
                _logger,
                _lexer,
                _parserRepository,
                _arithmeticAndRelationOperationService,
                textDocument);
        }
    }
}
