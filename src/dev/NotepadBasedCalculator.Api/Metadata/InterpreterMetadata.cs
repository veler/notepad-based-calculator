namespace NotepadBasedCalculator.Api
{
    public sealed class InterpreterMetadata : CultureCodeMetadata
    {
        public Type Type { get; }

        public InterpreterMetadata(IDictionary<string, object> metadata)
            : base(metadata)
        {
            if (metadata.TryGetValue(nameof(SupportedStatementTypeAttribute.Type), out object? value) && value is Type type)
            {
                Type = type;
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException($"A '{nameof(SupportedStatementTypeAttribute)}' or '{nameof(SupportedExpressionTypeAttribute)}' is expected when exporting an interpreter.");
            }
        }
    }
}
