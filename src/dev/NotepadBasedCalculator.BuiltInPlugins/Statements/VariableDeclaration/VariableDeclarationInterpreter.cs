using NotepadBasedCalculator.Api.Interpreter;

namespace NotepadBasedCalculator.BuiltInPlugins.Statements.VariableDeclaration
{
    [Export(typeof(IStatementInterpreter))]
    [SupportedStatementType(typeof(VariableDeclarationStatement))]
    internal class VariableDeclarationInterpreter : IStatementInterpreter
    {
        public Task<IData?> InterpretAsync(string culture, Statement statement, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
