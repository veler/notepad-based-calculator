namespace NotepadBasedCalculator.Api
{
    public sealed class ParserAndInterpreterMetadata : CultureCodeMetadata, IOrderableMetadata
    {
        public IReadOnlyList<string> Before { get; }

        public IReadOnlyList<string> After { get; }

        public string Name { get; }

        public ParserAndInterpreterMetadata(IDictionary<string, object> metadata)
            : base(metadata)
        {
            Before = metadata.GetValueOrDefault(nameof(OrderAttribute.Before)) as IReadOnlyList<string> ?? Array.Empty<string>();
            After = metadata.GetValueOrDefault(nameof(OrderAttribute.After)) as IReadOnlyList<string> ?? Array.Empty<string>();
            Name = metadata.GetValueOrDefault(nameof(NameAttribute.Name)) as string ?? string.Empty;
            Guard.IsNotEmpty(Name);

            if (Before.Count > 0)
            {
                var before = new List<string>();
                for (int i = 0; i < Before.Count; i++)
                {
                    if (!string.IsNullOrEmpty(Before[i]))
                    {
                        before.Add(Before[i]);
                    }
                }

                Before = before;
            }

            if (After.Count > 0)
            {
                var after = new List<string>();
                for (int i = 0; i < After.Count; i++)
                {
                    if (!string.IsNullOrEmpty(After[i]))
                    {
                        after.Add(After[i]);
                    }
                }

                After = after;
            }
        }
    }
}
