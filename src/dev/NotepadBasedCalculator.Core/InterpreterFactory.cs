using NotepadBasedCalculator.Api.Interpreter;

namespace NotepadBasedCalculator.Core
{
    [Export]
    internal sealed class InterpreterFactory
    {
        private readonly IEnumerable<Lazy<IStatementInterpreter, InterpreterMetadata>> _statementInterpreters;
        private readonly IEnumerable<Lazy<IExpressionInterpreter, InterpreterMetadata>> _expressionInterpreters;
        private readonly Lexer _lexer;
        private readonly Parser _parser;

        [ImportingConstructor]
        public InterpreterFactory(
            [ImportMany] IEnumerable<Lazy<IStatementInterpreter, InterpreterMetadata>> statementInterpreters,
            [ImportMany] IEnumerable<Lazy<IExpressionInterpreter, InterpreterMetadata>> expressionInterpreters,
            Lexer lexer,
            Parser parser)
        {
            _statementInterpreters = statementInterpreters;
            _expressionInterpreters = expressionInterpreters;
            _lexer = lexer;
            _parser = parser;
        }

        internal Interpreter CreateInterpreter(string culture, TextDocument textDocument)
        {
            return new Interpreter(
                culture,
                _lexer,
                _parser,
                _statementInterpreters,
                _expressionInterpreters,
                textDocument);
        }
    }
}
