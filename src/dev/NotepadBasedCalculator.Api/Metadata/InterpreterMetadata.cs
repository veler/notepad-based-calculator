namespace NotepadBasedCalculator.Api
{
    public sealed class InterpreterMetadata : CultureCodeMetadata
    {
        public Type[] Types { get; }

        public InterpreterMetadata(IDictionary<string, object> metadata)
            : base(metadata)
        {
            if (metadata.TryGetValue(nameof(SupportedStatementTypeAttribute.Type), out object? value) && value is not null)
            {
                if (value is Type type)
                {
                    Types = new[] { type };
                }
                else if (value is Type[] types)
                {
                    Types = types;
                }
                else
                {
                    ThrowHelper.ThrowInvalidDataException($"Unable to understand MEF's '{nameof(SupportedStatementTypeAttribute.Type)}' information.");
                }
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException($"A '{nameof(SupportedStatementTypeAttribute)}' or '{nameof(SupportedExpressionTypeAttribute)}' is expected when exporting an interpreter.");
            }
        }
    }
}
