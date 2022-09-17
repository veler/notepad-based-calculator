using System.Diagnostics;

namespace NotepadBasedCalculator.Api
{
    [DebuggerDisplay($"ResultedData = {{{nameof(ResultedData)}}}, ParsedExpression = {{{nameof(ParsedExpression)}}}, Error = {{{nameof(Error)}}}")]
    public record ExpressionParserAndInterpreterResult
    {
        public IData? ResultedData { get; set; } = null;

        public Expression? ParsedExpression { get; set; } = null;

        public LinkedToken? NextTokenToContinueWith { get; set; } = null;

        public DataOperationException? Error { get; set; } = null;
    }
}
