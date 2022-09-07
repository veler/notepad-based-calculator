namespace NotepadBasedCalculator.Api
{
    public sealed class FunctionInterpreterMetadata : CultureCodeMetadata
    {
        public string Name { get; }

        public FunctionInterpreterMetadata(IDictionary<string, object> metadata)
            : base(metadata)
        {
            if (metadata.TryGetValue(nameof(NameAttribute.Name), out object? value) && value is string name)
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
