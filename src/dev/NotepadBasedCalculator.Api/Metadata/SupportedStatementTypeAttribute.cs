namespace NotepadBasedCalculator.Api
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class SupportedStatementTypeAttribute : Attribute
    {
        public Type Type { get; }

        public SupportedStatementTypeAttribute(Type type)
        {
            Guard.IsNotNull(type);

            if (!typeof(Statement).IsAssignableFrom(type))
            {
                ThrowHelper.ThrowArgumentException(nameof(type), $"The type should inherits from '{nameof(Statement)}'.");
            }

            Type = type;
        }
    }
}
