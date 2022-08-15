namespace NotepadBasedCalculator.Api
{
    public abstract class ParserBase
    {
        private readonly Lazy<IParserRepository> _parserRepository;
        private readonly Lazy<ILogger> _logger;

        public IServiceProvider? ServiceProvider { get; set; }

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
            if (ignoreUnknownWords)
            {
                currentToken = DiscardWords(currentToken);
            }

            if (currentToken is null || currentToken.Token.IsNot(expectedTokenType))
            {
                nextToken = null;
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

        private void ThrowIfNoService()
        {
            if (ServiceProvider is null)
            {
                ThrowHelper.ThrowInvalidOperationException($"Please set the {nameof(ServiceProvider)} property. Generally, this property is set through a MEF import.");
            }
        }
    }
}
