namespace NotepadBasedCalculator.Api
{
    public sealed class LinkedToken
    {
        private readonly ITokenEnumerator _tokenEnumerator;

        private bool _nextTokenRetrieved;
        private LinkedToken? _nextToken;
        private int _tokenEndIndexWithCarriageReturn;

        public LinkedToken? Previous { get; }

        public LinkedToken? Next
        {
            get
            {
                DiscoverNextToken();
                return _nextToken;
            }
        }

        public Token Token { get; }

        internal int TokenEndIndexWithCarriageReturn
        {
            get
            {
                DiscoverNextToken();
                return _tokenEndIndexWithCarriageReturn;
            }
        }

        internal LinkedToken(LinkedToken? previous, Token token, ITokenEnumerator tokenEnumerator)
        {
            Guard.IsNotNull(token);
            Guard.IsNotNull(tokenEnumerator);

            Previous = previous;
            Token = token;
            _tokenEnumerator = tokenEnumerator;
        }

        public override string ToString()
        {
            return Token.ToString();
        }

        private void DiscoverNextToken()
        {
            if (!_nextTokenRetrieved)
            {
                if (_tokenEnumerator.MoveNext())
                {
                    Guard.IsNotNull(_tokenEnumerator.Current);
                    _nextToken
                        = new LinkedToken(
                            previous: this,
                            token: _tokenEnumerator.Current,
                            _tokenEnumerator);
                }

                Guard.IsNotNull(_tokenEnumerator.InternalCurrentToken);
                _tokenEndIndexWithCarriageReturn = _tokenEnumerator.InternalCurrentToken.EndIndex;
            }

            _nextTokenRetrieved = true;
        }
    }
}
