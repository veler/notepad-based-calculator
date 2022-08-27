namespace NotepadBasedCalculator.Api
{
    public class ExpressionParserAndInterpreterResult
    {
        public IData? ResultedData { get; set; } = null;

        public Expression? ParsedExpression { get; set; } = null;

        public LinkedToken? NextTokenToContinueWith { get; set; } = null;
    }
}
