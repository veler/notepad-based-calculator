namespace NotepadBasedCalculator.Api
{
    /// <summary>
    /// Basic class that represents an expression in a statement.
    /// </summary>
    public abstract class Expression
    {
        internal LinkedToken FirstToken { get; }

        internal LinkedToken LastToken { get; }

        protected Expression(LinkedToken firstToken, LinkedToken lastToken)
        {
            Guard.IsNotNull(firstToken);
            Guard.IsNotNull(lastToken);
            FirstToken = firstToken;
            LastToken = lastToken;
        }

        /// <summary>
        /// Gets a string representation of the expression.
        /// </summary>
        /// <returns>String that reprensents the expression</returns>
        public abstract override string ToString();
    }
}
