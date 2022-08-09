namespace NotepadBasedCalculator.Api
{
    public sealed class LinkedToken
    {
        private readonly Lazy<LinkedToken?> _nextToken;

        public LinkedToken? Previous { get; }

        public LinkedToken? Next => _nextToken.Value;

        public Token Token { get; }

        internal LinkedToken(LinkedToken? previous, Token token, ITokenEnumerator tokenEnumerator)
        {
            Guard.IsNotNull(token);
            Guard.IsNotNull(tokenEnumerator);

            Previous = previous;
            Token = token;

            _nextToken
                = new Lazy<LinkedToken?>(() =>
                {
                    if (tokenEnumerator.MoveNext())
                    {
                        Guard.IsNotNull(tokenEnumerator.Current);
                        return new LinkedToken(
                                previous: this,
                                token: tokenEnumerator.Current,
                                tokenEnumerator);
                    }

                    return null;
                });
        }

        public override string ToString()
        {
            return Token.ToString();
        }
    }
}
