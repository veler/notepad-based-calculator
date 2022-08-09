namespace NotepadBasedCalculator.Api
{
    public sealed class ExpressionParserMetadata
    {
        [DefaultValue(int.MaxValue)]
        public int Order { get; set; }

        [DefaultValue(null)]
        public string[]? CultureCode { get; set; }
    }
}
