namespace NotepadBasedCalculator.BuiltInPlugins.Statements.Function
{
    [Export(typeof(IStatementParser))]
    [Culture(SupportedCultures.Any)]
    internal sealed class FunctionStatementParser : ParserBase, IStatementParser
    {
        private readonly IFunctionDefinitionProvider _functionDefinitionProvider;
        private readonly IVariableService _variableService;

        [ImportingConstructor]
        public FunctionStatementParser(IFunctionDefinitionProvider functionDefinitionProvider, IVariableService variableService)
        {
            _functionDefinitionProvider = functionDefinitionProvider;
            _variableService = variableService;
        }

        public bool TryParseStatement(string culture, LinkedToken currentToken, out Statement? statement)
        {
            IReadOnlyList<FunctionDefinition> functionDefinitions = _functionDefinitionProvider.LoadFunctionDefinition(culture);

            // TODO:
            // Compare each function definition with the current line being parsed.
            // Attention => how to we support variable reference???? Use variableService?


            statement = null;
            return false;
        }
    }
}
