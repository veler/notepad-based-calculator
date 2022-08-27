namespace NotepadBasedCalculator.Api
{
    public sealed class FunctionStatement : Statement
    {
        public FunctionDefinition FunctionDefinition { get; }

        public FunctionStatement(FunctionDefinition functionDefinition, LinkedToken firstToken, LinkedToken lastToken)
            : base(firstToken, lastToken)
        {
            FunctionDefinition = functionDefinition;
        }

        public override string ToString()
        {
            return $"function {FunctionDefinition.FunctionFullName}()";
        }
    }
}
