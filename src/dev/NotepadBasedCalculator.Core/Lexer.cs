using System.Collections;
using NotepadBasedCalculator.Api;

namespace NotepadBasedCalculator.Core
{
    internal sealed class Lexer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="startIndex"></param>
        /// <returns>Returns the tokens in the line where <paramref name="startIndex"/> is, starting from the given position.</returns>
        internal LinkedToken? GetLineTokens(string? input, int startIndex = 0)
        {
            var tokenEnumerator = new TokenEnumerator(input, startIndex);
            if (tokenEnumerator.MoveNext())
            {
                Guard.IsNotNull(tokenEnumerator.Current);
                return new LinkedToken(previous: null, token: tokenEnumerator.Current, tokenEnumerator);
            }
            else if (tokenEnumerator.InternalCurrentToken is not null)
            {
                return new LinkedToken(previous: null, token: tokenEnumerator.InternalCurrentToken, tokenEnumerator);
            }

            return null;
        }

        private class TokenEnumerator : ITokenEnumerator
        {
            private readonly object _syncLock = new();
            private readonly string? _input;

            private bool _disposed;
            private int _currentPosition;
            private Token? _currentToken;

            public Token? Current
            {
                get
                {
                    lock (_syncLock)
                    {
                        ThrowIfDisposed();
                        return _currentToken;
                    }
                }
            }

            public Token? InternalCurrentToken { get; private set; }

            object? IEnumerator.Current => Current;

            public TokenEnumerator(string? input, int startIndex)
            {
                Guard.IsGreaterThanOrEqualTo(startIndex, 0);
                Guard.IsLessThanOrEqualTo(startIndex, input?.Length ?? 0);
                _input = input;
                _currentPosition = startIndex;
            }

            public void Dispose()
            {
                lock (_syncLock)
                {
                    _disposed = true;
                }
            }

            public bool MoveNext()
            {
                lock (_syncLock)
                {
                    ThrowIfDisposed();

                    if (string.IsNullOrEmpty(_input)
                        || _currentPosition >= _input!.Length)
                    {
                        return false;
                    }

                    Token token = DetectToken(_currentPosition);
                    InternalCurrentToken = token;

                    if (token.Type == TokenType.NewLine)
                    {
                        _currentToken = null;
                        return false;
                    }

                    _currentToken = token;
                    _currentPosition = token.EndIndex;
                    return true;
                }
            }

            public void Reset()
            {
                lock (_syncLock)
                {
                    ThrowIfDisposed();
                    _currentPosition = 0;
                    InternalCurrentToken = null;
                }
            }

            private void ThrowIfDisposed()
            {
                if (_disposed)
                {
                    ThrowHelper.ThrowObjectDisposedException(nameof(TokenEnumerator));
                }
            }

            private Token DetectToken(int startIndex)
            {
                Guard.IsNotNull(_input);
                char startChar = _input[startIndex];
                int endIndex = startIndex + 1;

                TokenType tokenType = DetectTokenType(startChar);

                if (_input.Length > startIndex)
                {
                    int nextCharIndex;
                    switch (tokenType)
                    {
                        case TokenType.Word:
                        case TokenType.Number:
                        case TokenType.Whitespace:
                            nextCharIndex = GetEndPositionOfRepeatedTokenType(startIndex, tokenType);
                            if (nextCharIndex > startIndex + 1)
                            {
                                endIndex = nextCharIndex;
                            }
                            break;

                        case TokenType.NewLine:
                            if (startChar == '\r')
                            {
                                nextCharIndex = startIndex + 1;
                                if (_input.Length > nextCharIndex)
                                {
                                    char nextChar = _input[nextCharIndex];
                                    if (nextChar == '\n')
                                    {
                                        endIndex = nextCharIndex + 1;
                                    }
                                }
                            }
                            break;

                        default:
                            break;
                    }
                }

                return new Token(_input, tokenType, startIndex, endIndex);
            }

            private int GetEndPositionOfRepeatedTokenType(int startIndex, TokenType tokenType)
            {
                Guard.IsNotNull(_input);
                int nextCharIndex = startIndex;
                do
                {
                    nextCharIndex++;
                } while (_input.Length > nextCharIndex && DetectTokenType(_input[nextCharIndex]) == tokenType);
                return nextCharIndex;
            }

            private static TokenType DetectTokenType(char c)
            {
                if (c == '\r' || c == '\n')
                {
                    return TokenType.NewLine;
                }

                if (char.IsDigit(c))
                {
                    return TokenType.Number;
                }

                if (char.IsWhiteSpace(c))
                {
                    return TokenType.Whitespace;
                }

                if (char.IsPunctuation(c)
                    || char.IsSymbol(c)
                    || c == 'π'
                    || c == '¾'
                    || c == '½'
                    || c == '¼'
                    || c == 'º'
                    || c == '¹'
                    || c == '²'
                    || c == '³'
                    || c == 'µ'
                    || c == '­'
                    || c == 'ª')
                {
                    return TokenType.SymbolOrPunctuation;
                }

                if (char.IsLetter(c))
                {
                    return TokenType.Word;
                }

                return TokenType.UnsupportedCharacter;
            }
        }
    }
}
