namespace NotepadBasedCalculator.Api
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class SupportedExpressionTypeAttribute : Attribute
    {
        public Type Type { get; }

        public SupportedExpressionTypeAttribute(Type type)
        {
            Guard.IsNotNull(type);

            if (!typeof(Expression).IsAssignableFrom(type))
            {
                ThrowHelper.ThrowArgumentException(nameof(type), $"The type should inherits from '{nameof(Expression)}'.");
            }

            Type = type;
        }
    }
}
