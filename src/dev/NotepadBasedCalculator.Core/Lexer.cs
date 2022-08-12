using System.Collections;

namespace NotepadBasedCalculator.Core
{
    internal static class Lexer
    {
        internal static readonly string[] LineBreakers = new[] { "\r\n", "\n" };

        internal static IReadOnlyList<TokenizedTextLine> Tokenize(string? wholeDocument)
        {
            var tokenizedLines = new List<TokenizedTextLine>();

            if (!string.IsNullOrEmpty(wholeDocument))
            {
                IReadOnlyList<string> lines = SplitLines(wholeDocument!);

                TokenizedTextLine? previousTokenizedLine = null;
                int i = 0;
                while (i < lines.Count)
                {
                    TokenizedTextLine tokenizedLine = TokenizeLineInternal(lines[i], previousTokenizedLine);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startPositionInDocument"></param>
        /// <param name="lineTextIncludingLineBreak"></param>
        /// <param name="knownData">An ordered list of <see cref="IData"/> that have already been parsed in the <paramref name="lineTextIncludingLineBreak"/>.</param>
        /// <returns></returns>
        internal static TokenizedTextLine TokenizeLine(int startPositionInDocument, string lineTextIncludingLineBreak, IReadOnlyList<IData>? knownData = null)
        {
            Guard.IsGreaterThanOrEqualTo(startPositionInDocument, 0);

            if (!string.IsNullOrEmpty(lineTextIncludingLineBreak))
            {
                return TokenizeLineInternal(startPositionInDocument, lineTextIncludingLineBreak, knownData);
            }
            else
            {
                return new TokenizedTextLine(startPositionInDocument, 0, lineTextIncludingLineBreak, null);
            }
        }

        private static TokenizedTextLine TokenizeLineInternal(string lineTextIncludingLineBreak, TokenizedTextLine? previousTokenizedLine, IReadOnlyList<IData>? knownData = null)
        {
            int lineStart = previousTokenizedLine?.EndIncludingLineBreak ?? 0;
            return TokenizeLineInternal(lineStart, lineTextIncludingLineBreak, knownData);
        }

        private static TokenizedTextLine TokenizeLineInternal(int startPositionInDocument, string lineTextIncludingLineBreak, IReadOnlyList<IData>? knownData = null)
        {
            Guard.IsGreaterThanOrEqualTo(startPositionInDocument, 0);

            var tokenEnumerator = new LineTokenEnumerator(lineTextIncludingLineBreak, knownData);

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
                startPositionInDocument,
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
            private readonly IReadOnlyList<IData>? _knownData;

            private bool _disposed;
            private int _currentPositionInLine;
            private IToken? _currentToken;

            public IToken? Current
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

            public LineTokenEnumerator(string lineTextIncludingLineBreak, IReadOnlyList<IData>? knownData)
            {
                Guard.IsNotNull(lineTextIncludingLineBreak);
                _lineTextIncludingLineBreak = lineTextIncludingLineBreak;
                _knownData = knownData;
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

                    IToken token = DetectToken(_currentPositionInLine);

                    if (token.Type == PredefinedTokenAndDataTypeNames.NewLine)
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

            private IToken DetectToken(int startIndex)
            {
                if (TryFindDataAtPosition(startIndex, out IData? foundData) && foundData is not null)
                {
                    return foundData;
                }

                char startChar = _lineTextIncludingLineBreak[startIndex];
                int endIndex = startIndex + 1;

                string tokenType = DetectTokenType(startChar);

                if (_lineTextIncludingLineBreak.Length > startIndex)
                {
                    int nextCharIndex;
                    switch (tokenType)
                    {
                        case PredefinedTokenAndDataTypeNames.Word:
                        case PredefinedTokenAndDataTypeNames.Whitespace:
                            nextCharIndex = GetEndPositionOfRepeatedTokenType(startIndex, tokenType);
                            if (nextCharIndex > startIndex + 1)
                            {
                                endIndex = nextCharIndex;
                            }
                            break;

                        case PredefinedTokenAndDataTypeNames.NewLine:
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

                return new Token(_lineTextIncludingLineBreak, startIndex, endIndex, tokenType);
            }

            private bool TryFindDataAtPosition(int startIndex, out IData? foundData)
            {
                if (_knownData is not null)
                {
                    // Use binary search to find the data at a given position in the line.
                    int min = 0;
                    int max = _knownData.Count - 1;
                    while (min <= max)
                    {
                        int middle = (min + max) / 2;
                        IData currentData = _knownData[middle];
                        if (startIndex == currentData.StartInLine)
                        {
                            foundData = currentData;
                            return true;
                        }
                        else if (startIndex < currentData.StartInLine)
                        {
                            max = middle - 1;
                        }
                        else
                        {
                            min = middle + 1;
                        }
                    }
                }

                foundData = null;
                return false;
            }

            private int GetEndPositionOfRepeatedTokenType(int startIndex, string tokenType)
            {
                int nextCharIndex = startIndex;
                do
                {
                    nextCharIndex++;
                } while (_lineTextIncludingLineBreak.Length > nextCharIndex && DetectTokenType(_lineTextIncludingLineBreak[nextCharIndex]) == tokenType);
                return nextCharIndex;
            }

            private static string DetectTokenType(char c)
            {
                if (c == '\r' || c == '\n')
                {
                    return PredefinedTokenAndDataTypeNames.NewLine;
                }

                if (char.IsDigit(c))
                {
                    return PredefinedTokenAndDataTypeNames.Digit;
                }

                if (char.IsWhiteSpace(c))
                {
                    return PredefinedTokenAndDataTypeNames.Whitespace;
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
                    return PredefinedTokenAndDataTypeNames.SymbolOrPunctuation;
                }

                if (char.IsLetter(c))
                {
                    return PredefinedTokenAndDataTypeNames.Word;
                }

                return PredefinedTokenAndDataTypeNames.UnsupportedCharacter;
            }
        }
    }
}
