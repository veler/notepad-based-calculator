namespace NotepadBasedCalculator.Api
{
    /// <summary>
    /// Basic class that represents a statement.
    /// </summary>
    public abstract class Statement
    {
        internal LinkedToken FirstToken { get; }

        internal LinkedToken LastToken { get; }

        protected Statement(LinkedToken firstToken, LinkedToken lastToken)
        {
            Guard.IsNotNull(firstToken);
            Guard.IsNotNull(lastToken);
            FirstToken = firstToken;
            LastToken = lastToken;
        }

        /// <summary>
        /// Gets a string representation of the statement.
        /// </summary>
        /// <returns>String that reprensents the statement</returns>
        public abstract override string ToString();
    }
}
