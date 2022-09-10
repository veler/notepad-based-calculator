namespace NotepadBasedCalculator.Api
{
    public record ExpressionParserAndInterpreterResult
    {
        public IData? ResultedData { get; set; } = null;

        public Expression? ParsedExpression { get; set; } = null;

        public LinkedToken? NextTokenToContinueWith { get; set; } = null;

        public DataOperationException? Error { get; set; } = null;
    }
}
