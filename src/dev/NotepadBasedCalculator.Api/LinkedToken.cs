namespace NotepadBasedCalculator.Api
{
    public sealed class LinkedToken
    {
        public LinkedToken? Previous { get; }

        public LinkedToken? Next { get; private set; }

        public Token Token { get; }

        private LinkedToken(LinkedToken? previous, LinkedToken? next, Token token)
        {
            Guard.IsNotNull(token);

            Previous = previous;
            Next = next;
            Token = token;
        }

        internal static LinkedToken? CreateFromList(IReadOnlyList<Token> tokens)
        {
            if (tokens is null || tokens.Count == 0)
            {
                return null;
            }

            LinkedToken result = null!;
            LinkedToken? previousToken = null;
            for (int i = 0; i < tokens.Count; i++)
            {
                var currentToken = new LinkedToken(previousToken, next: null, tokens[i]);
                if (previousToken != null)
                {
                    previousToken.Next = currentToken;
                }

                previousToken = currentToken;

                if (i == 0)
                {
                    result = currentToken;
                }
            }

            Guard.IsNotNull(result);
            return result;
        }
    }
}
