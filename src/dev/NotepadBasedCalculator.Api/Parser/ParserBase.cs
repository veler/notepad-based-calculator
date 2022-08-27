using System.Collections;

namespace NotepadBasedCalculator.Api
{
    public abstract class ParserBase
    {
        private readonly Lazy<IParserRepository> _parserRepository;
        private readonly Lazy<ILogger> _logger;

        public IServiceProvider? ServiceProvider { get; internal set; }

        protected ParserBase()
        {
            _parserRepository = new(() =>
            {
                ThrowIfNoService();
                if (ServiceProvider!.GetService(typeof(IParserRepository)) is IParserRepository parserRepository)
                {
                    return parserRepository;
                }
                ThrowHelper.ThrowInvalidOperationException();
                return null!;
            });

            _logger = new(() =>
            {
                ThrowIfNoService();
                if (ServiceProvider!.GetService(typeof(ILogger)) is ILogger logger)
                {
                    return logger;
                }
                ThrowHelper.ThrowInvalidOperationException();
                return null!;
            });
        }

        protected LinkedToken? JumpToNextTokenOfType(LinkedToken? currentToken, string expectedTokenType)
        {
            while (currentToken is not null)
            {
                if (currentToken.Token.Is(expectedTokenType))
                {
                    return currentToken;
                }

                currentToken = currentToken.Next;
            }

            return null;
        }

        protected LinkedToken? JumpToNextTokenOfType(LinkedToken? currentToken, string expectedTokenType, string expectedTokenText)
        {
            while (currentToken is not null)
            {
                if (currentToken.Token.Is(expectedTokenType, expectedTokenText))
                {
                    return currentToken;
                }

                currentToken = currentToken.Next;
            }

            return null;
        }

        /// <summary>
        /// Discard the current token and switch to the next one.
        /// </summary>
        /// <param name="currentToken">Current token being read.</param>
        /// <param name="expectedTokenType">The token type that is expect to be discarded.</param>
        protected bool DiscardToken(LinkedToken? currentToken, string expectedTokenType, out LinkedToken? nextToken)
        {
            return DiscardToken(currentToken, expectedTokenType, ignoreUnknownWords: true, out nextToken);
        }

        /// <summary>
        /// Discard the current token and switch to the next one.
        /// </summary>
        /// <param name="currentToken">Current token being read.</param>
        /// <param name="expectedTokenType">The token type that is expect to be discarded.</param>
        protected bool DiscardToken(LinkedToken? currentToken, string expectedTokenType, bool ignoreUnknownWords, out LinkedToken? nextToken)
        {
            LinkedToken? backupToken = currentToken;
            if (ignoreUnknownWords)
            {
                currentToken = DiscardWords(currentToken);
            }

            if (currentToken is null || currentToken.Token.IsNot(expectedTokenType))
            {
                nextToken = backupToken;
                return false;
            }

            nextToken = currentToken.Next;
            return true;
        }

        /// <summary>
        /// Discard the current token and switch to the next one.
        /// </summary>
        /// <param name="currentToken">Current token being read.</param>
        /// <param name="expectedTokenType">The token type that is expect to be discarded.</param>
        protected bool DiscardToken(LinkedToken? currentToken, string expectedTokenType, string expectedTokenText, out LinkedToken? nextToken)
        {
            return DiscardToken(currentToken, expectedTokenType, expectedTokenText, ignoreUnknownWords: true, out nextToken);
        }

        /// <summary>
        /// Discard the current token and switch to the next one.
        /// </summary>
        /// <param name="currentToken">Current token being read.</param>
        /// <param name="expectedTokenType">The token type that is expect to be discarded.</param>
        protected bool DiscardToken(LinkedToken? currentToken, string expectedTokenType, string expectedTokenText, bool ignoreUnknownWords, out LinkedToken? nextToken)
        {
            return DiscardToken(currentToken, expectedTokenType, expectedTokenText, StringComparison.OrdinalIgnoreCase, ignoreUnknownWords, out nextToken);
        }

        /// <summary>
        /// Discard the current token and switch to the next one.
        /// </summary>
        /// <param name="currentToken">Current token being read.</param>
        /// <param name="expectedTokenType">The token type that is expect to be discarded.</param>
        protected bool DiscardToken(LinkedToken? currentToken, string expectedTokenType, string expectedTokenText, StringComparison comparisonType, out LinkedToken? nextToken)
        {
            return DiscardToken(currentToken, expectedTokenType, expectedTokenText, comparisonType, ignoreUnknownWords: true, out nextToken);
        }

        /// <summary>
        /// Discard the current token and switch to the next one.
        /// </summary>
        /// <param name="currentToken">Current token being read.</param>
        /// <param name="expectedTokenType">The token type that is expect to be discarded.</param>
        protected bool DiscardToken(LinkedToken? currentToken, string expectedTokenType, string expectedTokenText, StringComparison comparisonType, bool ignoreUnknownWords, out LinkedToken? nextToken)
        {
            if (ignoreUnknownWords)
            {
                currentToken = DiscardWords(currentToken);
            }

            if (currentToken is not null && currentToken.Token.Is(expectedTokenType, expectedTokenText, comparisonType))
            {
                nextToken = currentToken.Next;
                return true;
            }

            nextToken = null;
            return false;
        }

        protected LinkedToken? DiscardWords(LinkedToken? currentToken)
        {
            while (currentToken is not null && currentToken.Token.Is(PredefinedTokenAndDataTypeNames.Word))
            {
                currentToken = currentToken.Next;
            }

            return currentToken;
        }

        protected Expression? ParseExpression(string expressionParserName, string culture, LinkedToken? currentToken, out LinkedToken? nextToken)
        {
            Guard.IsNotNullOrEmpty(expressionParserName);
            Guard.IsNotNull(culture);
            Expression? expression = null;

            if (currentToken is not null)
            {
                IExpressionParser? expressionParser = _parserRepository.Value.GetExpressionParser(culture, expressionParserName);
                if (expressionParser is not null)
                {
                    try
                    {
                        expressionParser.TryParseExpression(culture, currentToken, out expression);
                    }
                    catch (Exception ex)
                    {
                        _logger.Value.LogFault(
                            "ParserBase.ParseExpression.Fault",
                            ex,
                            ("ExpressionParserName", expressionParser.GetType().FullName));
                    }
                }
            }

            nextToken = expression?.LastToken.Next;
            return expression;
        }

        protected Expression? ParseExpression(string culture, LinkedToken? currentToken, out LinkedToken? nextToken)
        {
            Guard.IsNotNull(culture);
            Expression? expression = null;

            if (currentToken is not null)
            {
                foreach (IExpressionParser expressionParser in _parserRepository.Value.GetApplicableExpressionParsers(culture))
                {
                    try
                    {
                        if (expressionParser.TryParseExpression(culture, currentToken, out expression)
                        && expression is not null)
                        {
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Value.LogFault(
                            "ParserBase.ParseExpression.Fault",
                            ex,
                            ("ExpressionParserName", expressionParser.GetType().FullName));
                    }
                }
            }

            nextToken = expression?.LastToken.Next;
            return expression;
        }

        protected Expression? ParseExpression(string culture, LinkedToken? currentToken, string? stopParsingWhenTokenType, string? stopParsingWhenTokenText, out LinkedToken? nextToken)
        {
            Guard.IsNotNull(culture);
            Expression? expression = null;
            nextToken = null;

            if (currentToken is not null)
            {
                var tokenEnumerator = new TokenEnumerationWithStop(currentToken, stopParsingWhenTokenType, stopParsingWhenTokenText);
                Guard.IsNotNull(tokenEnumerator.Current);
                // TODO: should we dispose this enumerator at some point?

                var linkedToken
                    = new LinkedToken(
                        previous: null,
                        token: tokenEnumerator.Current,
                        tokenEnumerator);

                foreach (IExpressionParser expressionParser in _parserRepository.Value.GetApplicableExpressionParsers(culture))
                {
                    try
                    {
                        if (expressionParser.TryParseExpression(culture, linkedToken, out expression)
                            && expression is not null)
                        {
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Value.LogFault(
                            "ParserBase.ParseExpression.Fault",
                            ex,
                            ("ExpressionParserName", expressionParser.GetType().FullName));
                    }
                }

                nextToken = tokenEnumerator.CurrentLinkedToken;

                if ((nextToken is null
                    && !string.IsNullOrEmpty(stopParsingWhenTokenType)
                    && !string.IsNullOrEmpty(stopParsingWhenTokenText))
                    || (nextToken is not null
                    && string.IsNullOrEmpty(stopParsingWhenTokenType)
                    && string.IsNullOrEmpty(stopParsingWhenTokenText)))
                {
                    return null;
                }
            }

            return expression;
        }

        private void ThrowIfNoService()
        {
            if (ServiceProvider is null)
            {
                ThrowHelper.ThrowInvalidOperationException($"Please set the {nameof(ServiceProvider)} property. Generally, this property is set through a MEF import.");
            }
        }

        /// <summary>
        /// A token enumerator that stops enumerating once it hits a given token type & text.
        /// </summary>
        private class TokenEnumerationWithStop : ITokenEnumerator
        {
            private readonly object _syncLock = new();
            private readonly LinkedToken _originalStartToken;
            private readonly string? _stopParsingWhenTokenType;
            private readonly string? _stopParsingWhenTokenText;

            private bool _disposed;
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

            internal LinkedToken? CurrentLinkedToken { get; private set; }

            object? IEnumerator.Current => Current;

            public TokenEnumerationWithStop(LinkedToken originalStartToken, string? stopParsingWhenTokenType, string? stopParsingWhenTokenText)
            {
                Guard.IsNotNull(originalStartToken);
                _originalStartToken = originalStartToken;
                _stopParsingWhenTokenType = stopParsingWhenTokenType;
                _stopParsingWhenTokenText = stopParsingWhenTokenText;
                CurrentLinkedToken = originalStartToken;
                _currentToken = originalStartToken.Token;
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
                    Guard.IsNotNull(CurrentLinkedToken);

                    CurrentLinkedToken = CurrentLinkedToken.Next;

                    if (CurrentLinkedToken is null
                        || CurrentLinkedToken.Token.Is(_stopParsingWhenTokenType ?? string.Empty, _stopParsingWhenTokenText ?? string.Empty))
                    {
                        _currentToken = null;
                        return false;
                    }

                    _currentToken = CurrentLinkedToken.Token;
                    return true;
                }
            }

            public void Reset()
            {
                lock (_syncLock)
                {
                    CurrentLinkedToken = _originalStartToken;
                    _currentToken = _originalStartToken.Token;
                }
            }

            private void ThrowIfDisposed()
            {
                if (_disposed)
                {
                    ThrowHelper.ThrowObjectDisposedException(nameof(TokenEnumerationWithStop));
                }
            }
        }
    }
}
