using System.Diagnostics;

namespace NotepadBasedCalculator.Api
{
    [DebuggerDisplay($"ParsedStatement = {{{nameof(ParsedStatement)}}}, Error = {{{nameof(Error)}}}, ResultedData = [{{{nameof(ResultedData)}}}]")]
    public record StatementParserAndInterpreterResult
    {
        public IData? ResultedData { get; set; } = null;

        public Statement? ParsedStatement { get; set; } = null;

        public DataOperationException? Error { get; set; } = null;
    }
}
