namespace NotepadBasedCalculator.BuiltInPlugins.Expressions.NumericalCalculus
{
    [Export(typeof(IExpressionParser))]
    [Name(PredefinedExpressionParserNames.NumericalExpression)]
    [Culture(SupportedCultures.Any)]
    [Order(int.MaxValue - 1)]
    internal sealed class NumericalCalculusExpressionParser : ParserBase, IExpressionParser
    {
        public bool TryParseExpression(string culture, LinkedToken currentToken, out Expression? expression)
        {
            expression = ParseAdditiveExpression(culture, currentToken);
            return expression is not null;
        }

        /// <summary>
        /// Parse expression that contains multiply, division and modulus symbols.
        /// 
        /// Corresponding grammar :
        ///     Multiplicative_Expression (('+' | '-') Multiplicative_Expression)*
        /// </summary>
        private Expression? ParseAdditiveExpression(string culture, LinkedToken currentToken)
        {
            Expression? expression = ParseMultiplicativeExpression(culture, currentToken, out LinkedToken? nextToken);

            if (expression is not null)
            {
                LinkedToken? operatorToken = DiscardWords(nextToken);
                while (operatorToken is not null)
                {
                    BinaryOperatorType binaryOperator;
                    if (operatorToken.Token.Is(PredefinedTokenAndDataTypeNames.AdditionOperator))
                    {
                        binaryOperator = BinaryOperatorType.Addition;
                    }
                    else if (operatorToken.Token.Is(PredefinedTokenAndDataTypeNames.SubstractionOperator))
                    {
                        binaryOperator = BinaryOperatorType.Subtraction;
                    }
                    else
                    {
                        return expression;
                    }

                    Expression? rightExpression = ParseMultiplicativeExpression(culture, operatorToken.Next, out nextToken);

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
        /// Parse expression that contains multiply, division and modulus symbols.
        /// 
        /// Corresponding grammar :
        ///     Primary_Expression (('*' | '/') Primary_Expression)*
        /// </summary>
        private Expression? ParseMultiplicativeExpression(string culture, LinkedToken? currentToken, out LinkedToken? nextToken)
        {
            Expression? expression = ParserPrimaryExpression(culture, currentToken, out nextToken);

            if (expression is not null)
            {
                LinkedToken? operatorToken = DiscardWords(nextToken);
                while (operatorToken is not null)
                {
                    BinaryOperatorType binaryOperator;
                    LinkedToken? expressionStartToken = operatorToken.Next;
                    if (operatorToken.Token.Is(PredefinedTokenAndDataTypeNames.MultiplicationOperator))
                    {
                        binaryOperator = BinaryOperatorType.Multiply;
                    }
                    else if (operatorToken.Token.Is(PredefinedTokenAndDataTypeNames.DivisionOperator))
                    {
                        binaryOperator = BinaryOperatorType.Division;
                    }
                    else if (operatorToken.Token.Is(PredefinedTokenAndDataTypeNames.LeftParenth))
                    {
                        binaryOperator = BinaryOperatorType.Multiply;
                        expressionStartToken = operatorToken;
                    }
                    else if (operatorToken.Token.Is(PredefinedTokenAndDataTypeNames.Numeric))
                    {
                        if (operatorToken.Token is INumericData numericData && numericData.IsNegative)
                        {
                            binaryOperator = BinaryOperatorType.Addition;
                        }
                        else
                        {
                            binaryOperator = BinaryOperatorType.Multiply;
                        }

                        expressionStartToken = operatorToken;
                    }
                    else
                    {
                        return expression;
                    }

                    Expression? rightExpression = ParserPrimaryExpression(culture, expressionStartToken, out nextToken);

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
        /// Parse an expression that can be either a primitive data, a variable reference or an expression between parenthesis.
        /// 
        /// Corresponding grammar :
        ///     Primitive_Value
        ///     | Identifier
        ///     | '(' Expression ')'
        /// </summary>
        private Expression? ParserPrimaryExpression(string culture, LinkedToken? currentToken, out LinkedToken? nextToken)
        {
            currentToken = DiscardWords(currentToken);
            if (currentToken is not null)
            {
                // Detect Numbers, Percentage, Dates...etc.
                if (currentToken.Token is IData data)
                {
                    nextToken = currentToken.Next;
                    return new DataExpression(currentToken, currentToken, data);
                }

                // Detect variable reference
                if (currentToken.Token.Is(PredefinedTokenAndDataTypeNames.VariableReference))
                {
                    nextToken = currentToken.Next;
                    return new VariableReferenceExpression(currentToken);
                }

                // Detect expression between parenthesis.
                LinkedToken leftParenthToken = currentToken;
                if (DiscardLeftParenth(leftParenthToken, out nextToken))
                {
                    Expression? parsedExpression = ParseExpression(culture, nextToken, out nextToken);

                    LinkedToken? rightParenthToken = DiscardWords(nextToken);
                    if (parsedExpression is not null && DiscardRightParenth(rightParenthToken, out nextToken) && rightParenthToken is not null)
                    {
                        parsedExpression = new GroupExpression(leftParenthToken, rightParenthToken, parsedExpression);
                        return parsedExpression;
                    }
                }
            }

            nextToken = null;
            return null;
        }

        private bool DiscardLeftParenth(LinkedToken? currentToken, out LinkedToken? nextToken)
        {
            return DiscardToken(
                currentToken,
                PredefinedTokenAndDataTypeNames.LeftParenth,
                ignoreUnknownWords: true,
                out nextToken);
        }

        private bool DiscardRightParenth(LinkedToken? currentToken, out LinkedToken? nextToken)
        {
            return DiscardToken(
                currentToken,
                PredefinedTokenAndDataTypeNames.RightParenth,
                ignoreUnknownWords: true,
                out nextToken);
        }
    }
}
