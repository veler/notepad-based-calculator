namespace NotepadBasedCalculator.Api
{
    public sealed class ParserMetadata : CultureCodeMetadata
    {
        public int Order { get; }

        public string Name { get; }

        public ParserMetadata(IDictionary<string, object> metadata)
            : base(metadata)
        {
            if (metadata.TryGetValue(nameof(OrderAttribute.Order), out object? value) && value is int order)
            {
                Order = order;
            }
            else
            {
                Order = 0;
            }

            if (metadata.TryGetValue(nameof(NameAttribute.Name), out value) && value is string name)
            {
                Name = name;
            }
            else
            {
                Name = string.Empty;
            }
        }
    }
}
