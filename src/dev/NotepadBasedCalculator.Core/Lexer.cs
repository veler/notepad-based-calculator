using System.Collections;

namespace NotepadBasedCalculator.Core
{
    internal sealed class Lexer
    {
        internal static readonly string[] LineBreakers = new[] { "\r\n", "\n" };

        internal IReadOnlyList<TokenizedTextLine> Tokenize(string? input)
        {
            var tokenizedLines = new List<TokenizedTextLine>();

            if (!string.IsNullOrEmpty(input))
            {
                IReadOnlyList<string> lines = SplitLines(input!);

                TokenizedTextLine? previousTokenizedLine = null;
                int i = 0;
                while (i < lines.Count)
                {
                    TokenizedTextLine tokenizedLine = TokenizeLine(lines[i], previousTokenizedLine);
                    tokenizedLines.Add(tokenizedLine);

                    previousTokenizedLine = tokenizedLine;
                    i++;
                }
            }

            if (tokenizedLines.Count == 0)
            {
                tokenizedLines.Add(
                    new TokenizedTextLine(
                        0,
                        0,
                        string.Empty,
                        null));
            }

            return tokenizedLines;
        }

        private static TokenizedTextLine TokenizeLine(string lineTextIncludingLineBreak, TokenizedTextLine? previousTokenizedLine)
        {
            var tokenEnumerator = new LineTokenEnumerator(lineTextIncludingLineBreak);

            int lineStart = previousTokenizedLine?.EndIncludingLineBreak ?? 0;

            int lineBreakLength = 0;
            for (int i = 0; i < LineBreakers.Length; i++)
            {
                string lineBreaker = LineBreakers[i];
                if (lineTextIncludingLineBreak.EndsWith(lineBreaker, StringComparison.Ordinal))
                {
                    lineBreakLength = lineBreaker.Length;
                    break;
                }
            }

            LinkedToken? linkedToken = null;
            if (tokenEnumerator.MoveNext() && tokenEnumerator.Current is not null)
            {
                linkedToken
                    = new LinkedToken(
                        previous: null,
                        token: tokenEnumerator.Current,
                        tokenEnumerator);
            }

            return new TokenizedTextLine(
                lineStart,
                lineBreakLength,
                lineTextIncludingLineBreak,
                linkedToken);
        }

        /// <summary>
        /// Split an <paramref name="input"/> per lines and keep the break line in the result.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static IReadOnlyList<string> SplitLines(string input)
        {
            var lines = new List<string>() { input };

            for (int i = 0; i < LineBreakers.Length; i++)
            {
                string delimiter = LineBreakers[i];
                for (int j = 0; j < lines.Count; j++)
                {
                    int index = lines[j].IndexOf(delimiter);
                    if (index > -1
                        && lines[j].Length > index + 1)
                    {
                        string leftPart = lines[j].Substring(0, index + delimiter.Length);
                        string rightPart = lines[j].Substring(index + delimiter.Length);
                        lines[j] = leftPart;
                        lines.Insert(j + 1, rightPart);
                    }
                }
            }

            return lines;
        }

        private class LineTokenEnumerator : ITokenEnumerator
        {
            private readonly object _syncLock = new();
            private readonly string _lineTextIncludingLineBreak;

            private bool _disposed;
            private int _currentPositionInLine;
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

            object? IEnumerator.Current => Current;

            public LineTokenEnumerator(string lineTextIncludingLineBreak)
            {
                Guard.IsNotNull(lineTextIncludingLineBreak);
                _lineTextIncludingLineBreak = lineTextIncludingLineBreak;
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

                    if (string.IsNullOrEmpty(_lineTextIncludingLineBreak)
                        || _currentPositionInLine >= _lineTextIncludingLineBreak.Length)
                    {
                        _currentToken = null;
                        return false;
                    }

                    Token token = DetectToken(_currentPositionInLine);

                    if (token.Type == TokenType.NewLine)
                    {
                        _currentToken = null;
                        return false;
                    }

                    _currentToken = token;
                    _currentPositionInLine = token.EndInLine;
                    return true;
                }
            }

            public void Reset()
            {
                lock (_syncLock)
                {
                    ThrowIfDisposed();
                    _currentPositionInLine = 0;
                }
            }

            private void ThrowIfDisposed()
            {
                if (_disposed)
                {
                    ThrowHelper.ThrowObjectDisposedException(nameof(LineTokenEnumerator));
                }
            }

            private Token DetectToken(int startIndex)
            {
                char startChar = _lineTextIncludingLineBreak[startIndex];
                int endIndex = startIndex + 1;

                TokenType tokenType = DetectTokenType(startChar);

                if (_lineTextIncludingLineBreak.Length > startIndex)
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
                                if (_lineTextIncludingLineBreak.Length > nextCharIndex)
                                {
                                    char nextChar = _lineTextIncludingLineBreak[nextCharIndex];
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

                return new Token(_lineTextIncludingLineBreak, tokenType, startIndex, endIndex);
            }

            private int GetEndPositionOfRepeatedTokenType(int startIndex, TokenType tokenType)
            {
                int nextCharIndex = startIndex;
                do
                {
                    nextCharIndex++;
                } while (_lineTextIncludingLineBreak.Length > nextCharIndex && DetectTokenType(_lineTextIncludingLineBreak[nextCharIndex]) == tokenType);
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
