namespace NotepadBasedCalculator.BuiltInPlugins.Expressions.NumericalCalculus
{
    [Export(typeof(IExpressionParser))]
    [Name(PredefinedExpressionParserNames.NumericalCalculusExpression)]
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
                LinkedToken? operatorToken = JumpToNextTokenOfType(nextToken, PredefinedTokenAndDataTypeNames.SymbolOrPunctuation);
                while (operatorToken is not null)
                {
                    BinaryOperatorType binaryOperator;
                    if (operatorToken.Token.Is(PredefinedTokenAndDataTypeNames.SymbolOrPunctuation, "+"))
                    {
                        binaryOperator = BinaryOperatorType.Addition;
                    }
                    else if (operatorToken.Token.Is(PredefinedTokenAndDataTypeNames.SymbolOrPunctuation, "-"))
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
                        operatorToken = JumpToNextTokenOfType(nextToken, PredefinedTokenAndDataTypeNames.SymbolOrPunctuation);
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
                LinkedToken? operatorToken = JumpToNextTokenOfType(nextToken, PredefinedTokenAndDataTypeNames.SymbolOrPunctuation);
                while (operatorToken is not null)
                {
                    BinaryOperatorType binaryOperator;
                    if (operatorToken.Token.Is(PredefinedTokenAndDataTypeNames.SymbolOrPunctuation, "*"))
                    {
                        binaryOperator = BinaryOperatorType.Multiply;
                    }
                    else if (operatorToken.Token.Is(PredefinedTokenAndDataTypeNames.SymbolOrPunctuation, "/"))
                    {
                        binaryOperator = BinaryOperatorType.Division;
                    }
                    else
                    {
                        return expression;
                    }

                    Expression? rightExpression = ParserPrimaryExpression(culture, operatorToken.Next, out nextToken);

                    if (rightExpression is not null)
                    {
                        expression = new BinaryOperatorExpression(expression, binaryOperator, rightExpression);
                        operatorToken = JumpToNextTokenOfType(nextToken, PredefinedTokenAndDataTypeNames.SymbolOrPunctuation);
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
            //while (currentToken is not null)
            {
                // Detect Numbers, Percentage, Dates...etc.

                currentToken = DiscardWords(currentToken);
                if (currentToken is not null)
                {
                    if (currentToken.Token.Is(PredefinedTokenAndDataTypeNames.Numeric) && currentToken.Token is IData data)
                    {
                        nextToken = currentToken.Next;
                        return new DataExpression(currentToken, currentToken, data);
                    }

                    //  TODO: Detect variable reference.

                    if (DiscardLeftParenth(currentToken, out nextToken))
                    {
                        Expression? parsedExpression = ParseExpression(culture, nextToken, out nextToken);

                        if (parsedExpression is not null && DiscardRightParenth(nextToken, out nextToken))
                        {
                            return parsedExpression;
                        }
                    }
                }

                // Could be a word we just want to ignore. Go to the next token.
                //    currentToken = currentToken.Next;
            }

            nextToken = null;
            return null;
        }

        private bool DiscardLeftParenth(LinkedToken? currentToken, out LinkedToken? nextToken)
        {
            return DiscardToken(
                currentToken,
                PredefinedTokenAndDataTypeNames.SymbolOrPunctuation,
                "(",
                ignoreUnknownWords: true,
                out nextToken);
        }

        private bool DiscardRightParenth(LinkedToken? currentToken, out LinkedToken? nextToken)
        {
            return DiscardToken(
                currentToken,
                PredefinedTokenAndDataTypeNames.SymbolOrPunctuation,
                ")",
                ignoreUnknownWords: true,
                out nextToken);
        }
    }
}
