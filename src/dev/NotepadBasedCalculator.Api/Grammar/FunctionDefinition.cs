namespace NotepadBasedCalculator.Api
{
    public sealed class FunctionDefinition
    {
        public string FunctionFullName { get; }

        public LinkedToken TokenizedFunctionDefinition { get; }

        public FunctionDefinition(string functionFullName, LinkedToken tokenizedFunctionDefinition)
        {
            Guard.IsNotNullOrWhiteSpace(functionFullName);
            Guard.IsNotNull(tokenizedFunctionDefinition);
            FunctionFullName = functionFullName;
            TokenizedFunctionDefinition = tokenizedFunctionDefinition;
        }

        public override string ToString()
        {
            return FunctionFullName;
        }
    }
}
