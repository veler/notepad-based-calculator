namespace NotepadBasedCalculator.BuiltInPlugins.Expressions.Conditional
{
    [Export(typeof(IExpressionParser))]
    [Culture(SupportedCultures.Any)]
    internal sealed class ConditionalExpressionParser : ParserBase, IExpressionParser
    {
        public bool TryParseExpression(string culture, LinkedToken currentToken, out Expression? expression)
        {
            expression = ParseEqualityExpression(culture, currentToken);
            return expression is not null;
        }

        /// <summary>
        /// Parse expression that contains equality symbols.
        /// 
        /// Corresponding grammar :
        ///     Relational_Expression (('=' | '!=') Relational_Expression)*
        /// </summary>
        private Expression? ParseEqualityExpression(string culture, LinkedToken? currentToken)
        {
            Expression? expression = ParseRelationalExpression(culture, currentToken, out LinkedToken? nextToken);

            if (expression is not null)
            {
                LinkedToken? operatorToken = DiscardWords(nextToken);
                while (operatorToken is not null)
                {
                    BinaryOperatorType binaryOperator;
                    if (operatorToken.Token.Is(PredefinedTokenAndDataTypeNames.IsEqualToOperator))
                    {
                        binaryOperator = BinaryOperatorType.Equality;
                    }
                    else if (operatorToken.Token.Is(PredefinedTokenAndDataTypeNames.IsNotEqualToOperator))
                    {
                        binaryOperator = BinaryOperatorType.NoEquality;
                    }
                    else
                    {
                        return expression;
                    }

                    Expression? rightExpression = ParseRelationalExpression(culture, nextToken, out nextToken);
                    if (rightExpression is not null)
                    {
                        expression = new BinaryOperatorExpression(expression, binaryOperator, rightExpression);
                        operatorToken = DiscardWords(nextToken);
                    }
                    else
                    {
                        return expression;
                    }
                }
            }

            return expression;
        }

        /// <summary>
        /// Parse expression that contains lesser than or greater than symbols.
        /// 
        /// Corresponding grammar :
        ///     Additive_Expression (('<' | '>' | '<=' | '>=') Additive_Expression)*
        /// </summary>
        private Expression? ParseRelationalExpression(string culture, LinkedToken? currentToken, out LinkedToken? nextToken)
        {
            Expression? expression = ParseExpression(PredefinedExpressionParserNames.NumericalCalculusExpression, culture, currentToken, out nextToken);

            if (expression is not null)
            {
                LinkedToken? operatorToken = DiscardWords(nextToken);
                while (operatorToken is not null)
                {
                    BinaryOperatorType binaryOperator;
                    if (operatorToken.Token.Is(PredefinedTokenAndDataTypeNames.LessThanOrEqualToOperator))
                    {
                        binaryOperator = BinaryOperatorType.LessThanOrEqualTo;
                    }
                    else if (operatorToken.Token.Is(PredefinedTokenAndDataTypeNames.LessThanOperator))
                    {
                        binaryOperator = BinaryOperatorType.LessThan;
                    }
                    else if (operatorToken.Token.Is(PredefinedTokenAndDataTypeNames.GreaterThanOrEqualToOperator))
                    {
                        binaryOperator = BinaryOperatorType.GreaterThanOrEqualTo;
                    }
                    else if (operatorToken.Token.Is(PredefinedTokenAndDataTypeNames.GreaterThanOperator))
                    {
                        binaryOperator = BinaryOperatorType.GreaterThan;
                    }
                    else
                    {
                        return expression;
                    }

                    Expression? rightExpression = ParseExpression(PredefinedExpressionParserNames.NumericalCalculusExpression, culture, nextToken, out nextToken);
                    if (rightExpression is not null)
                    {
                        expression = new BinaryOperatorExpression(expression, binaryOperator, rightExpression);
                        operatorToken = DiscardWords(nextToken);
                    }
                    else
                    {
                        return expression;
                    }
                }
            }

            return expression;
        }
    }
}
