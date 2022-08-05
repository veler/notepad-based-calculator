namespace NotepadBasedCalculator.Api
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class OrderAttribute : ExportAttribute
    {
        public int? Order { get; private set; }

        public OrderAttribute(int order)
        {
            Order = order;
        }
    }
}
