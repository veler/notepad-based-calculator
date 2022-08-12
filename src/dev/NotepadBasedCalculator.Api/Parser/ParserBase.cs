namespace NotepadBasedCalculator.Api
{
    public abstract class ParserBase
    {
        [ImportMany]
        private IEnumerable<Lazy<IExpressionParser, ExpressionParserMetadata>>? ExpressionParsers { get; }

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

        protected Expression? ParseExpression(LinkedToken linkedToken, string culture)
        {
            if (ExpressionParsers is null)
            {
                ThrowHelper.ThrowInvalidOperationException("Unable to find any expression parser to use. Make sure to MEF export the class.");
            }

            Expression? expression = null;

            foreach (Lazy<IExpressionParser, ExpressionParserMetadata>? expressionParser in ExpressionParsers)
            {
                if (expressionParser.Value.TryParseExpression(culture, linkedToken, out expression)
                    && expression is not null)
                {
                    break;
                }
            }

            return expression;
        }

        protected LinkedToken? DiscardWhitespaces(LinkedToken? currentToken)
        {
            while (currentToken is not null && currentToken.Token.Is(PredefinedTokenAndDataTypeNames.Whitespace))
            {
                currentToken = currentToken.Next;
            }

            return currentToken;
        }
    }
}
