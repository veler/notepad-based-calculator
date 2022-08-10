namespace NotepadBasedCalculator.Api
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DataTypeAttribute : Attribute
    {
        public string DataType { get; }

        public DataTypeAttribute(string dataType)
        {
            Guard.IsNotNullOrWhiteSpace(dataType);
            DataType = dataType;
        }
    }
}
