namespace NotepadBasedCalculator.Api
{
    public sealed class ParserMetadata : CultureCodeMetadata
    {
        [DefaultValue(int.MaxValue)]
        public int Order { get; }

        public ParserMetadata(IDictionary<string, object> metadata)
            : base(metadata)
        {
            if (metadata.TryGetValue(nameof(OrderAttribute.Order), out object? value) && value is int order)
            {
                Order = order;
            }
        }
    }
}
