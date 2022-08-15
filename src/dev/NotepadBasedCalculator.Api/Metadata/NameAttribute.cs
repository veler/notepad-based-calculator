namespace NotepadBasedCalculator.Api
{
    /// <summary>
    /// Defines the internal name of this component. This name can be used to explicitly request this component to be invoked.
    /// </summary>
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class NameAttribute : Attribute
    {
        public string Name { get; }

        public NameAttribute(string name)
        {
            Name = name;
        }
    }
}
