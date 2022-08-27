﻿using System.Collections;

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

        public async Task<bool> TryParseAndInterpretExpressionAsync(
            string culture,
            LinkedToken? currentToken,
            string? parseUntilTokenIsOfType,
            string? parseUntilTokenHasText,
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
                var tokenEnumerator = new TokenEnumerationWithStop(currentToken, parseUntilTokenIsOfType, parseUntilTokenHasText);
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

        /// <summary>
        /// A token enumerator that stops enumerating once it hits a given token type & text.
        /// </summary>
        private class TokenEnumerationWithStop : ITokenEnumerator
        {
            private readonly object _syncLock = new();
            private readonly LinkedToken _originalStartToken;
            private readonly string? _parseUntilTokenIsOfType;
            private readonly string? _parseUntilTokenHasText;

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

            public TokenEnumerationWithStop(LinkedToken originalStartToken, string? parseUntilTokenIsOfType, string? parseUntilTokenHasText)
            {
                Guard.IsNotNull(originalStartToken);
                _originalStartToken = originalStartToken;
                _parseUntilTokenIsOfType = parseUntilTokenIsOfType;
                _parseUntilTokenHasText = parseUntilTokenHasText;
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
                        || CurrentLinkedToken.Token.Is(_parseUntilTokenIsOfType ?? string.Empty, _parseUntilTokenHasText ?? string.Empty))
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