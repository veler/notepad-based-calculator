namespace NotepadBasedCalculator.Api
{
    /// <summary>
    /// Defines the priority of this component over others.
    /// </summary>
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class OrderAttribute : Attribute
    {
        private string _before = string.Empty;
        private string _after = string.Empty;

        /// <summary>
        /// Gets or sets the internal name of a component to compare with.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the internal name of a component to compare with.
        /// </summary>
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
