using System.Collections;

namespace NotepadBasedCalculator.Core
{
    [Export(typeof(IParserAndInterpreterService))]
    internal sealed class ParserAndInterpreterService : IParserAndInterpreterService
    {
        private readonly ILogger _logger;
        private readonly IParserRepository _parserRepository;

        [ImportingConstructor]
        public ParserAndInterpreterService(ILogger logger, IParserRepository parserRepository)
        {
            _logger = logger;
            _parserRepository = parserRepository;
        }

        public async Task<bool> TryParseAndInterpretExpressionAsync(
            string culture,
            LinkedToken? currentToken,
            IVariableService variableService,
            ExpressionParserAndInterpreterResult result,
            CancellationToken cancellationToken)
        {
            if (currentToken is not null)
            {
                Guard.IsNotNull(culture);
                foreach (IExpressionParserAndInterpreter parserAndInterpreter in _parserRepository.GetApplicableExpressionParsersAndInterpreters(culture))
                {
                    bool expressionFound
                        = await TryParseAndInterpretExpressionInternalAsync(
                            culture,
                            currentToken,
                            parserAndInterpreter,
                            variableService,
                            result,
                            cancellationToken);

                    if (expressionFound)
                    {
                        result.NextTokenToContinueWith = result.ParsedExpression!.LastToken.Next;
                        return true;
                    }
                }
            }

            result.ParsedExpression = null;
            result.ResultedData = null;
            result.NextTokenToContinueWith = currentToken;
            return false;
        }

        public async Task<bool> TryParseAndInterpretExpressionAsync(
            string expressionParserAndInterpreterName,
            string culture,
            LinkedToken? currentToken,
            IVariableService variableService,
            ExpressionParserAndInterpreterResult result,
            CancellationToken cancellationToken)
        {
            bool expressionFound = false;

            if (currentToken is not null)
            {
                Guard.IsNotNull(culture);
                Guard.IsNotNullOrEmpty(expressionParserAndInterpreterName);

                IExpressionParserAndInterpreter parserAndInterpreter
                    = _parserRepository.GetExpressionParserAndInterpreter(
                        culture,
                        expressionParserAndInterpreterName);

                expressionFound
                    = await TryParseAndInterpretExpressionInternalAsync(
                        culture,
                        currentToken,
                        parserAndInterpreter,
                        variableService,
                        result,
                        cancellationToken);
            }

            if (!expressionFound)
            {
                result.ParsedExpression = null;
                result.ResultedData = null;
                result.NextTokenToContinueWith = currentToken;
            }
            else
            {
                result.NextTokenToContinueWith = result.ParsedExpression!.LastToken.Next;
            }
            return expressionFound;
        }

        public Task<bool> TryParseAndInterpretExpressionAsync(
            string culture,
            LinkedToken? currentToken,
            string? parseUntilTokenIsOfType,
            string? parseUntilTokenHasText,
            IVariableService variableService,
            ExpressionParserAndInterpreterResult result,
            CancellationToken cancellationToken)
        {
            return TryParseAndInterpretExpressionAsync(
                culture,
                currentToken,
                parseUntilTokenIsOfType,
                parseUntilTokenHasText,
                string.Empty,
                variableService,
                result,
                cancellationToken);
        }

        public async Task<bool> TryParseAndInterpretExpressionAsync(
            string culture,
            LinkedToken? currentToken,
            string? parseUntilTokenIsOfType,
            string? parseUntilTokenHasText,
            string? nestedTokenType,
            IVariableService variableService,
            ExpressionParserAndInterpreterResult result,
            CancellationToken cancellationToken)
        {
            Guard.IsNotNull(result);
            bool expressionFound = false;
            result.NextTokenToContinueWith = null;

            if (currentToken is not null)
            {
                Guard.IsNotNull(culture);
                var tokenEnumerator
                    = new TokenEnumerationWithStop(
                        currentToken,
                        parseUntilTokenIsOfType,
                        parseUntilTokenHasText,
                        nestedTokenType);

                Guard.IsNotNull(tokenEnumerator.Current);
                // TODO: should we dispose this enumerator at some point?

                var linkedToken
                    = new LinkedToken(
                        previous: null,
                        token: tokenEnumerator.Current,
                        tokenEnumerator);

                foreach (IExpressionParserAndInterpreter parserAndInterpreter in _parserRepository.GetApplicableExpressionParsersAndInterpreters(culture))
                {
                    expressionFound
                        = await TryParseAndInterpretExpressionInternalAsync(
                            culture,
                            linkedToken,
                            parserAndInterpreter,
                            variableService,
                            result,
                            cancellationToken);

                    if (expressionFound)
                    {
                        break;
                    }
                }

                result.NextTokenToContinueWith = tokenEnumerator.CurrentLinkedToken;

                if ((result.NextTokenToContinueWith is null
                    && !string.IsNullOrEmpty(parseUntilTokenIsOfType)
                    && !string.IsNullOrEmpty(parseUntilTokenHasText))
                    || (result.NextTokenToContinueWith is not null
                    && string.IsNullOrEmpty(parseUntilTokenIsOfType)
                    && string.IsNullOrEmpty(parseUntilTokenHasText)))
                {
                    expressionFound = false;
                }
            }

            if (!expressionFound)
            {
                result.ParsedExpression = null;
                result.ResultedData = null;
            }
            return expressionFound;
        }

        public Task<bool> TryParseAndInterpretExpressionAsync(
            string expressionParserAndInterpreterName,
            string culture,
            LinkedToken? currentToken,
            string? parseUntilTokenIsOfType,
            string? parseUntilTokenHasText,
            IVariableService variableService,
            ExpressionParserAndInterpreterResult result,
            CancellationToken cancellationToken)
        {
            return TryParseAndInterpretExpressionAsync(
                expressionParserAndInterpreterName,
                culture,
                currentToken,
                parseUntilTokenIsOfType,
                parseUntilTokenHasText,
                string.Empty,
                variableService,
                result,
                cancellationToken);
        }

        public async Task<bool> TryParseAndInterpretExpressionAsync(
            string expressionParserAndInterpreterName,
            string culture,
            LinkedToken? currentToken,
            string? parseUntilTokenIsOfType,
            string? parseUntilTokenHasText,
            string? nestedTokenType,
            IVariableService variableService,
            ExpressionParserAndInterpreterResult result,
            CancellationToken cancellationToken)
        {
            Guard.IsNotNull(result);
            bool expressionFound = false;
            result.NextTokenToContinueWith = null;

            if (currentToken is not null)
            {
                Guard.IsNotNull(culture);
                var tokenEnumerator
                    = new TokenEnumerationWithStop(
                        currentToken,
                        parseUntilTokenIsOfType,
                        parseUntilTokenHasText,
                        nestedTokenType);

                Guard.IsNotNull(tokenEnumerator.Current);
                // TODO: should we dispose this enumerator at some point?

                var linkedToken
                    = new LinkedToken(
                        previous: null,
                        token: tokenEnumerator.Current,
                        tokenEnumerator);

                Guard.IsNotNullOrEmpty(expressionParserAndInterpreterName);

                IExpressionParserAndInterpreter parserAndInterpreter
                    = _parserRepository.GetExpressionParserAndInterpreter(
                        culture,
                        expressionParserAndInterpreterName);

                expressionFound
                    = await TryParseAndInterpretExpressionInternalAsync(
                        culture,
                        linkedToken,
                        parserAndInterpreter,
                        variableService,
                        result,
                        cancellationToken);

                result.NextTokenToContinueWith = tokenEnumerator.CurrentLinkedToken;

                if ((result.NextTokenToContinueWith is null
                    && !string.IsNullOrEmpty(parseUntilTokenIsOfType)
                    && !string.IsNullOrEmpty(parseUntilTokenHasText))
                    || (result.NextTokenToContinueWith is not null
                    && string.IsNullOrEmpty(parseUntilTokenIsOfType)
                    && string.IsNullOrEmpty(parseUntilTokenHasText)))
                {
                    expressionFound = false;
                }
            }

            if (!expressionFound)
            {
                result.ParsedExpression = null;
                result.ResultedData = null;
            }
            return expressionFound;
        }

        public async Task<bool> TryParseAndInterpretStatementAsync(
            string culture,
            LinkedToken? currentToken,
            string? parseUntilTokenIsOfType,
            string? parseUntilTokenHasText,
            IVariableService variableService,
            StatementParserAndInterpreterResult result,
            CancellationToken cancellationToken)
        {
            Guard.IsNotNull(result);
            bool statementFound = false;

            if (currentToken is not null)
            {
                Guard.IsNotNull(culture);
                var tokenEnumerator
                    = new TokenEnumerationWithStop(
                        currentToken,
                        parseUntilTokenIsOfType,
                        parseUntilTokenHasText,
                        string.Empty);
                Guard.IsNotNull(tokenEnumerator.Current);
                // TODO: should we dispose this enumerator at some point?

                var linkedToken
                    = new LinkedToken(
                        previous: null,
                        token: tokenEnumerator.Current,
                        tokenEnumerator);

                foreach (IStatementParserAndInterpreter parserAndInterpreter in _parserRepository.GetApplicableStatementParsersAndInterpreters(culture))
                {
                    statementFound
                        = await TryParseAndInterpretStatementInternalAsync(
                            culture,
                            linkedToken,
                            parserAndInterpreter,
                            variableService,
                            result,
                            cancellationToken);

                    if (statementFound)
                    {
                        break;
                    }
                }

                if ((result.ParsedStatement?.LastToken.Next is null
                    && !string.IsNullOrEmpty(parseUntilTokenIsOfType)
                    && !string.IsNullOrEmpty(parseUntilTokenHasText))
                    || (result.ParsedStatement?.LastToken.Next is not null
                    && string.IsNullOrEmpty(parseUntilTokenIsOfType)
                    && string.IsNullOrEmpty(parseUntilTokenHasText)))
                {
                    statementFound = false;
                }
            }

            if (!statementFound)
            {
                result.ParsedStatement = null;
                result.ResultedData = null;
            }
            return statementFound;
        }

        private async Task<bool> TryParseAndInterpretExpressionInternalAsync(
            string culture,
            LinkedToken currentToken,
            IExpressionParserAndInterpreter parserAndInterpreter,
            IVariableService variableService,
            ExpressionParserAndInterpreterResult result,
            CancellationToken cancellationToken)
        {
            Guard.IsNotNull(culture);
            Guard.IsNotNull(parserAndInterpreter);
            Guard.IsNotNull(variableService);
            Guard.IsNotNull(result);
            result.ParsedExpression = null;
            result.ResultedData = null;

            try
            {
                bool expressionFound
                    = await parserAndInterpreter.TryParseAndInterpretExpressionAsync(
                        culture,
                        currentToken,
                        variableService,
                        result,
                        cancellationToken);

                if (expressionFound)
                {
                    if (result.ParsedExpression is null)
                    {
                        ThrowHelper.ThrowInvalidDataException(
                            $"The method '{nameof(parserAndInterpreter.TryParseAndInterpretExpressionAsync)}' returned true " +
                            $"but '{nameof(ExpressionParserAndInterpreterResult)}.{nameof(ExpressionParserAndInterpreterResult.ParsedExpression)}' is null. " +
                            $"It should not be null.");
                    }

                    return true;
                }
            }
            catch (OperationCanceledException)
            {
                // Ignore.
            }
            catch (DataOperationException doe)
            {
                result.Error = doe;
            }
            catch (Exception ex)
            {
                _logger.LogFault(
                    "ParserBase.ParseExpression.Fault",
                    ex,
                    ("ExpressionParserName", parserAndInterpreter.GetType().FullName));
            }

            result.ParsedExpression = null;
            result.ResultedData = null;
            return false;
        }

        private async Task<bool> TryParseAndInterpretStatementInternalAsync(
            string culture,
            LinkedToken currentToken,
            IStatementParserAndInterpreter parserAndInterpreter,
            IVariableService variableService,
            StatementParserAndInterpreterResult result,
            CancellationToken cancellationToken)
        {
            Guard.IsNotNull(culture);
            Guard.IsNotNull(parserAndInterpreter);
            Guard.IsNotNull(variableService);
            Guard.IsNotNull(result);
            result.ParsedStatement = null;
            result.ResultedData = null;

            try
            {
                bool expressionFound
                    = await parserAndInterpreter.TryParseAndInterpretStatementAsync(
                        culture,
                        currentToken,
                        variableService,
                        result,
                        cancellationToken);

                if (expressionFound)
                {
                    if (result.ParsedStatement is null)
                    {
                        ThrowHelper.ThrowInvalidDataException(
                            $"The method '{nameof(parserAndInterpreter.TryParseAndInterpretStatementAsync)}' returned true " +
                            $"but '{nameof(StatementParserAndInterpreterResult)}.{nameof(StatementParserAndInterpreterResult.ParsedStatement)}' is null. " +
                            $"It should not be null.");
                    }

                    return true;
                }
            }
            catch (OperationCanceledException)
            {
                // Ignore.
            }
            catch (DataOperationException doe)
            {
                result.Error = doe;
            }
            catch (Exception ex)
            {
                _logger.LogFault(
                    "ParserBase.ParseStatement.Fault",
                    ex,
                    ("StatementParserName", parserAndInterpreter.GetType().FullName));
            }

            result.ParsedStatement = null;
            result.ResultedData = null;
            return false;
        }

        /// <summary>
        /// A token enumerator that stops enumerating once it hits a given token type & text.
        /// </summary>
        private class TokenEnumerationWithStop : ITokenEnumerator
        {
            private readonly object _syncLock = new();
            private readonly LinkedToken _originalStartToken;
            private readonly string _parseUntilTokenIsOfType;
            private readonly string _parseUntilTokenHasText;
            private readonly string _nestedMarkTokenType;

            private bool _disposed;
            private int _nestedPartCount = 0;
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

            public TokenEnumerationWithStop(
                LinkedToken originalStartToken,
                string? parseUntilTokenIsOfType,
                string? parseUntilTokenHasText,
                string? nestedMarkTokenType)
            {
                Guard.IsNotNull(originalStartToken);
                _originalStartToken = originalStartToken;
                _parseUntilTokenIsOfType = parseUntilTokenIsOfType ?? string.Empty;
                _parseUntilTokenHasText = parseUntilTokenHasText ?? string.Empty;
                _nestedMarkTokenType = nestedMarkTokenType ?? string.Empty;
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

                    if (CurrentLinkedToken is null)
                    {
                        _currentToken = null;
                        return false;
                    }

                    _currentToken = CurrentLinkedToken.Token;

                    if (CurrentLinkedToken.Token.IsOfType(_nestedMarkTokenType))
                    {
                        _nestedPartCount++;
                    }

                    if (string.IsNullOrEmpty(_parseUntilTokenHasText))
                    {
                        if (CurrentLinkedToken.Token.IsOfType(_parseUntilTokenIsOfType ?? string.Empty))
                        {
                            if (_nestedPartCount > 0)
                            {
                                _nestedPartCount--;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        if (CurrentLinkedToken.Token.Is(_parseUntilTokenIsOfType ?? string.Empty, _parseUntilTokenHasText!))
                        {
                            if (_nestedPartCount > 0)
                            {
                                _nestedPartCount--;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }

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
