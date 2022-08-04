namespace NotepadBasedCalculator.Api
{
    public class TokenDefinition
    {
        /// <summary>
        /// Gets a non-localized name for identifying this token.
        /// </summary>
        public string Identifier { get; }

        public TokenDefinition(string identifier)
        {
            Guard.IsNotNullOrEmpty(identifier);
            Identifier = identifier;
        }
    }
}
