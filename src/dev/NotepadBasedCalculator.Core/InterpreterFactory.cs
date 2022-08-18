namespace NotepadBasedCalculator.Core
{
    [Export]
    internal sealed class InterpreterFactory
    {
        private readonly Lexer _lexer;
        private readonly Parser _parser;

        [ImportingConstructor]
        public InterpreterFactory(Lexer lexer, Parser parser)
        {
            _lexer = lexer;
            _parser = parser;
        }

        internal Interpreter CreateInterpreter(string culture, TextDocument textDocument)
        {
            return new Interpreter(culture, _lexer, _parser, textDocument);
        }
    }
}
