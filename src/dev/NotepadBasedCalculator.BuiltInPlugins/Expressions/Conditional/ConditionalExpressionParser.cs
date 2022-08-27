namespace NotepadBasedCalculator.BuiltInPlugins.Expressions.Conditional
{
    [Export(typeof(IExpressionParser))]
    [Name(PredefinedExpressionParserNames.ConditionalExpression)]
    [Culture(SupportedCultures.Any)]
    internal sealed class ConditionalExpressionParser : ParserBase, IExpressionParser
    {
        public bool TryParseExpression(string culture, LinkedToken currentToken, out Expression? expression)
        {
            expression = ParseEqualityAndRelationalExpression(culture, currentToken);
            return expression is not null;
        }

        /// <summary>
        /// Parse expression that contains equality symbols.
        /// 
        /// Corresponding grammar :
        ///     NumericalCalculus_Expression (('==' | '!=' | '<' | '>' | '<=' | '>=') NumericalCalculus_Expression)
        /// </summary>
        private Expression? ParseEqualityAndRelationalExpression(string culture, LinkedToken? currentToken)
        {
            Expression? expression = ParseExpression(PredefinedExpressionParserNames.NumericalExpression, culture, currentToken, out LinkedToken? nextToken);

            if (expression is not null)
            {
                LinkedToken? operatorToken = DiscardWords(nextToken);

                if (operatorToken is not null)
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
                    else if (operatorToken.Token.Is(PredefinedTokenAndDataTypeNames.LessThanOrEqualToOperator))
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

                    Expression? rightExpression = ParseExpression(PredefinedExpressionParserNames.NumericalExpression, culture, operatorToken.Next, out _);
                    if (rightExpression is not null)
                    {
                        expression = new BinaryOperatorExpression(expression, binaryOperator, rightExpression);
                    }
                }
            }

            return expression;
        }
    }
}
