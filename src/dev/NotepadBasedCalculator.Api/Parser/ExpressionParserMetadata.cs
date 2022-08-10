#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace NotepadBasedCalculator.Api
{
    public sealed class ExpressionParserMetadata
    {
        [DefaultValue(int.MaxValue)]
        public int Order { get; }

        [DefaultValue("")]
        public string[] CultureCode { get; }
    }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
