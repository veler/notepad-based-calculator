using System.Diagnostics;

namespace NotepadBasedCalculator.Api
{
    [DebuggerDisplay($"Token = {{{nameof(Token)}.{nameof(IToken.GetText)}()}}")]
    public sealed class LinkedToken
    {
        private readonly Lazy<LinkedToken?> _nextToken;

        public LinkedToken? Previous { get; }

        public LinkedToken? Next => _nextToken.Value;

        public IToken Token { get; }

        internal LinkedToken(LinkedToken? previous, IToken token, ITokenEnumerator tokenEnumerator)
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
                    else if (tokenEnumerator.Current is not null)
                    {
                        return new LinkedToken(
                                previous: this,
                                token: tokenEnumerator.Current);
                    }

                    return null;
                });
        }

        private LinkedToken(LinkedToken? previous, IToken token)
        {
            Guard.IsNotNull(token);
            Previous = previous;
            Token = token;
            _nextToken = new Lazy<LinkedToken?>(() => null);
        }

        public override string? ToString()
        {
            return Token.ToString();
        }
    }
}
