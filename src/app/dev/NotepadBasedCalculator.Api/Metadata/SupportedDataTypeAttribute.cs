namespace NotepadBasedCalculator.Api
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class SupportedDataTypeAttribute : Attribute
    {
        public Type Type { get; }

        public SupportedDataTypeAttribute(Type type)
        {
            Guard.IsNotNull(type);

            if (!typeof(IData).IsAssignableFrom(type))
            {
                ThrowHelper.ThrowArgumentException(nameof(type), $"The type should inherits from '{nameof(IData)}'.");
            }

            Type = type;
        }
    }
}
