namespace NotepadBasedCalculator.BuiltInPlugins.Expressions.Conditional
{
    [Export(typeof(IExpressionParser))]
    [Culture(SupportedCultures.Any)]
    internal sealed class ConditionalExpressionParser : ParserBase, IExpressionParser
    {
        public bool TryParseExpression(string culture, LinkedToken currentToken, out Expression? expression)
        {
            expression = ParseConditionalOrExpression(culture, currentToken);
            return expression is not null;
        }

        /// <summary>
        /// Parse expression that contains a logical 'or' operator.
        /// 
        /// Corresponding grammar :
        ///     Conditional_And_Expression ('OR' Conditional_And_Expression)*
        /// </summary>
        private Expression? ParseConditionalOrExpression(string culture, LinkedToken currentToken)
        {
            Expression? expression = ParseConditionalAndExpression(culture, currentToken, out LinkedToken? nextToken);

            if (expression is not null)
            {
                LinkedToken? operatorToken = JumpToNextTokenOfType(nextToken, PredefinedTokenAndDataTypeNames.Word, "or");
                while (operatorToken is not null)
                {
                    Expression? rightExpression = ParseConditionalAndExpression(culture, operatorToken.Next, out nextToken);
                    if (rightExpression is not null)
                    {
                        expression = new BinaryOperatorExpression(expression, BinaryOperatorType.LogicalOr, rightExpression);
                        operatorToken = JumpToNextTokenOfType(nextToken, PredefinedTokenAndDataTypeNames.Word, "or");
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
        /// Parse expression that contains a logical 'and' operator.
        /// 
        /// Corresponding grammar :
        ///     Equality_Expression ('AND' Equality_Expression)*
        /// </summary>
        private Expression? ParseConditionalAndExpression(string culture, LinkedToken? currentToken, out LinkedToken? nextToken)
        {
            Expression? expression = ParseEqualityExpression(culture, currentToken, out nextToken);

            if (expression is not null)
            {
                LinkedToken? operatorToken = JumpToNextTokenOfType(nextToken, PredefinedTokenAndDataTypeNames.Word, "and");
                while (operatorToken is not null)
                {
                    Expression? rightExpression = ParseEqualityExpression(culture, operatorToken.Next, out nextToken);
                    if (rightExpression is not null)
                    {
                        expression = new BinaryOperatorExpression(expression, BinaryOperatorType.LogicalAnd, rightExpression);
                        operatorToken = JumpToNextTokenOfType(nextToken, PredefinedTokenAndDataTypeNames.Word, "and");
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
        /// Parse expression that contains equality symbols.
        /// 
        /// Corresponding grammar :
        ///     Relational_Expression (('=' | '!=') Relational_Expression)*
        /// </summary>
        private Expression? ParseEqualityExpression(string culture, LinkedToken? currentToken, out LinkedToken? nextToken)
        {
            Expression? expression = ParseRelationalExpression(culture, currentToken, out nextToken);

            if (expression is not null)
            {
                while (nextToken is not null)
                {
                    // TODO: can be optimized.
                    BinaryOperatorType binaryOperator;
                    bool isNotEqualToOperator = IsNotEqualToOperator(nextToken, out LinkedToken? nextTokenAfterNotEqualOperator);
                    bool isEqualToOperator = IsEqualToOperator(nextToken, out LinkedToken? nextTokenAfterEqualOperator);

                    if (isNotEqualToOperator && !isEqualToOperator)
                    {
                        binaryOperator = BinaryOperatorType.NoEquality;
                        nextToken = nextTokenAfterNotEqualOperator;
                    }
                    else if (!isNotEqualToOperator && isEqualToOperator)
                    {
                        binaryOperator = BinaryOperatorType.Equality;
                        nextToken = nextTokenAfterEqualOperator;
                    }
                    else
                    {
                        return expression;
                    }

                    Expression? rightExpression = ParseRelationalExpression(culture, nextToken, out nextToken);
                    if (rightExpression is not null)
                    {
                        expression = new BinaryOperatorExpression(expression, binaryOperator, rightExpression);
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
                while (nextToken is not null)
                {
                    // TODO: can be optimized.
                    BinaryOperatorType binaryOperator;
                    bool isLessThanOrEqualToOperator = IsLessThanOrEqualToOperator(nextToken, out LinkedToken? nextTokenAfterLessThanOrEqualToOperator);
                    bool isGreaterThanOrEqualToOperator = IsGreaterThanOrEqualToOperator(nextToken, out LinkedToken? nextTokenAfterGreaterThanOrEqualToOperator);
                    bool isLessThanOperator = IsLessThanOperator(nextToken, out LinkedToken? nextTokenAfterLessThanOperator);
                    bool isGreaterThanOperator = IsGreaterThanOperator(nextToken, out LinkedToken? nextTokenAfterGreaterThanOperator);

                    if (isLessThanOrEqualToOperator)
                    {
                        binaryOperator = BinaryOperatorType.LessThanOrEqualTo;
                        nextToken = nextTokenAfterLessThanOrEqualToOperator;
                    }
                    else if (isGreaterThanOrEqualToOperator)
                    {
                        binaryOperator = BinaryOperatorType.GreaterThanOrEqualTo;
                        nextToken = nextTokenAfterGreaterThanOrEqualToOperator;
                    }
                    else if (isLessThanOperator)
                    {
                        binaryOperator = BinaryOperatorType.LessThan;
                        nextToken = nextTokenAfterLessThanOperator;
                    }
                    else if (isGreaterThanOperator)
                    {
                        binaryOperator = BinaryOperatorType.GreaterThan;
                        nextToken = nextTokenAfterGreaterThanOperator;
                    }
                    else
                    {
                        return expression;
                    }

                    Expression? rightExpression = ParseExpression(PredefinedExpressionParserNames.NumericalCalculusExpression, culture, nextToken, out nextToken);
                    if (rightExpression is not null)
                    {
                        expression = new BinaryOperatorExpression(expression, binaryOperator, rightExpression);
                    }
                    else
                    {
                        return expression;
                    }
                }
            }

            return expression;
        }

        private bool IsEqualToOperator(LinkedToken? currentToken, out LinkedToken? nextToken)
        {
            return
                DiscardToken(
                    currentToken,
                    PredefinedTokenAndDataTypeNames.SymbolOrPunctuation,
                    "=",
                    ignoreUnknownWords: true,
                    out nextToken)
                && nextToken is not null
                && DiscardToken(
                    currentToken,
                    PredefinedTokenAndDataTypeNames.SymbolOrPunctuation,
                    "=",
                    ignoreUnknownWords: false,
                    out nextToken)
                && nextToken is not null;
        }

        private bool IsNotEqualToOperator(LinkedToken? currentToken, out LinkedToken? nextToken)
        {
            return
                DiscardToken(
                    currentToken,
                    PredefinedTokenAndDataTypeNames.SymbolOrPunctuation,
                    "!",
                    ignoreUnknownWords: true,
                    out nextToken)
                && nextToken is not null
                && DiscardToken(
                    currentToken,
                    PredefinedTokenAndDataTypeNames.SymbolOrPunctuation,
                    "=",
                    ignoreUnknownWords: false,
                    out nextToken)
                && nextToken is not null;
        }

        private bool IsLessThanOperator(LinkedToken? currentToken, out LinkedToken? nextToken)
        {
            return
                DiscardToken(
                    currentToken,
                    PredefinedTokenAndDataTypeNames.SymbolOrPunctuation,
                    "<",
                    ignoreUnknownWords: true,
                    out nextToken)
                && nextToken is not null;
        }

        private bool IsLessThanOrEqualToOperator(LinkedToken? currentToken, out LinkedToken? nextToken)
        {
            return
                DiscardToken(
                    currentToken,
                    PredefinedTokenAndDataTypeNames.SymbolOrPunctuation,
                    "<",
                    ignoreUnknownWords: true,
                    out nextToken)
                && nextToken is not null
                && DiscardToken(
                    currentToken,
                    PredefinedTokenAndDataTypeNames.SymbolOrPunctuation,
                    "=",
                    ignoreUnknownWords: false,
                    out nextToken)
                && nextToken is not null;
        }

        private bool IsGreaterThanOperator(LinkedToken? currentToken, out LinkedToken? nextToken)
        {
            return
                DiscardToken(
                    currentToken,
                    PredefinedTokenAndDataTypeNames.SymbolOrPunctuation,
                    ">",
                    ignoreUnknownWords: true,
                    out nextToken)
                && nextToken is not null;
        }

        private bool IsGreaterThanOrEqualToOperator(LinkedToken? currentToken, out LinkedToken? nextToken)
        {
            return
                DiscardToken(
                    currentToken,
                    PredefinedTokenAndDataTypeNames.SymbolOrPunctuation,
                    ">",
                    ignoreUnknownWords: true,
                    out nextToken)
                && nextToken is not null
                && DiscardToken(
                    currentToken,
                    PredefinedTokenAndDataTypeNames.SymbolOrPunctuation,
                    "=",
                    ignoreUnknownWords: false,
                    out nextToken)
                && nextToken is not null;
        }
    }
}
