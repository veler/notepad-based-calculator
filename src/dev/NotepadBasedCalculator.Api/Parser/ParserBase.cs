namespace NotepadBasedCalculator.Api
{
    public abstract class ParserBase
    {
        private readonly Lazy<IParserRepository> _parserRepository;

        public IServiceProvider? ServiceProvider { get; set; }

        protected ParserBase()
        {
            _parserRepository = new(() =>
            {
                if (ServiceProvider is null)
                {
                    ThrowHelper.ThrowInvalidOperationException($"Please set the {nameof(ServiceProvider)} property. Generally, this property is set through a MEF import.");
                }

                return (IParserRepository)ServiceProvider.GetService(typeof(IParserRepository));
            });
        }

        /// <summary>
        /// Discard the current token and switch to the next one.
        /// </summary>
        /// <param name="currentToken">Current token being read.</param>
        /// <param name="expectedTokenType">The token type that is expect to be discarded.</param>
        protected bool DiscardToken(LinkedToken? currentToken, string expectedTokenType, out LinkedToken? nextToken)
        {
            return DiscardToken(currentToken, expectedTokenType, ignoreWhitespace: true, out nextToken);
        }

        /// <summary>
        /// Discard the current token and switch to the next one.
        /// </summary>
        /// <param name="currentToken">Current token being read.</param>
        /// <param name="expectedTokenType">The token type that is expect to be discarded.</param>
        protected bool DiscardToken(LinkedToken? currentToken, string expectedTokenType, bool ignoreWhitespace, out LinkedToken? nextToken)
        {
            if (ignoreWhitespace)
            {
                currentToken = DiscardWhitespaces(currentToken);
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
            return DiscardToken(currentToken, expectedTokenType, expectedTokenText, ignoreWhitespace: true, out nextToken);
        }

        /// <summary>
        /// Discard the current token and switch to the next one.
        /// </summary>
        /// <param name="currentToken">Current token being read.</param>
        /// <param name="expectedTokenType">The token type that is expect to be discarded.</param>
        protected bool DiscardToken(LinkedToken? currentToken, string expectedTokenType, string expectedTokenText, bool ignoreWhitespace, out LinkedToken? nextToken)
        {
            return DiscardToken(currentToken, expectedTokenType, expectedTokenText, StringComparison.OrdinalIgnoreCase, ignoreWhitespace, out nextToken);
        }

        /// <summary>
        /// Discard the current token and switch to the next one.
        /// </summary>
        /// <param name="currentToken">Current token being read.</param>
        /// <param name="expectedTokenType">The token type that is expect to be discarded.</param>
        protected bool DiscardToken(LinkedToken? currentToken, string expectedTokenType, string expectedTokenText, StringComparison comparisonType, out LinkedToken? nextToken)
        {
            return DiscardToken(currentToken, expectedTokenType, expectedTokenText, comparisonType, ignoreWhitespace: true, out nextToken);
        }

        /// <summary>
        /// Discard the current token and switch to the next one.
        /// </summary>
        /// <param name="currentToken">Current token being read.</param>
        /// <param name="expectedTokenType">The token type that is expect to be discarded.</param>
        protected bool DiscardToken(LinkedToken? currentToken, string expectedTokenType, string expectedTokenText, StringComparison comparisonType, bool ignoreWhitespace, out LinkedToken? nextToken)
        {
            if (ignoreWhitespace)
            {
                currentToken = DiscardWhitespaces(currentToken);
            }

            if (currentToken is not null && currentToken.Token.Is(expectedTokenType, expectedTokenText, comparisonType))
            {
                nextToken = currentToken.Next;
                return true;
            }

            nextToken = null;
            return false;
        }

        protected LinkedToken? DiscardWhitespaces(LinkedToken? currentToken)
        {
            while (currentToken is not null && currentToken.Token.Is(PredefinedTokenAndDataTypeNames.Whitespace))
            {
                currentToken = currentToken.Next;
            }

            return currentToken;
        }

        protected Expression? ParseExpression(LinkedToken linkedToken, string culture)
        {
            Expression? expression = null;

            foreach (IExpressionParser expressionParser in _parserRepository.Value.GetApplicableExpressionParsers(culture))
            {
                if (expressionParser.TryParseExpression(culture, linkedToken, out expression)
                    && expression is not null)
                {
                    break;
                }
            }

            return expression;
        }
    }
}
