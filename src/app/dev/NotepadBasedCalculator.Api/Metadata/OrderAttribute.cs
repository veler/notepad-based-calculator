namespace NotepadBasedCalculator.Api
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class OrderAttribute : Attribute
    {
        private string _before = string.Empty;
        private string _after = string.Empty;

        public string Before
        {
            get
            {
                return _before;
            }
            set
            {
                Guard.IsNotNullOrEmpty(value);
                _before = value;
            }
        }

        public string After
        {
            get
            {
                return _after;
            }
            set
            {
                Guard.IsNotNullOrEmpty(value);
                _after = value;
            }
        }
    }
}
