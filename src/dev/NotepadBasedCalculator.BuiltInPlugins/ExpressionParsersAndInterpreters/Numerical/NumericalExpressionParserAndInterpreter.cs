namespace NotepadBasedCalculator.BuiltInPlugins.ExpressionParsersAndInterpreters.Numerical
{
    [Export(typeof(IExpressionParserAndInterpreter))]
    [Name(PredefinedExpressionParserNames.NumericalExpression)]
    [Culture(SupportedCultures.Any)]
    [Order(int.MaxValue - 2)]
    [Shared]
    internal sealed class NumericalExpressionParserAndInterpreter : IExpressionParserAndInterpreter
    {
        [Import]
        public IParserAndInterpreterService ParserAndInterpreterService { get; set; } = null!;

        public Task<bool> TryParseAndInterpretExpressionAsync(
            string culture,
            LinkedToken currentToken,
            IVariableService variableService,
            ExpressionParserAndInterpreterResult result,
            CancellationToken cancellationToken)
        {
            return
                ParseAdditiveExpressionAsync(
                    culture,
                    currentToken,
                    variableService,
                    result,
                    cancellationToken);
        }

        /// <summary>
        /// Parse expression that contains multiply, division and modulus symbols.
        /// 
        /// Corresponding grammar :
        ///     Multiplicative_Expression (('+' | '-') Multiplicative_Expression)*
        /// </summary>
        private async Task<bool> ParseAdditiveExpressionAsync(
            string culture,
            LinkedToken currentToken,
            IVariableService variableService,
            ExpressionParserAndInterpreterResult result,
            CancellationToken cancellationToken)
        {
            bool foundLeftExpression
                = await ParseMultiplicativeExpressionAsync(
                    culture,
                    currentToken,
                    variableService,
                    result,
                    cancellationToken);

            if (foundLeftExpression)
            {
                LinkedToken? operatorToken = result.NextTokenToContinueWith.SkipNextWordTokens();
                while (operatorToken is not null)
                {
                    BinaryOperatorType binaryOperator;
                    if (operatorToken.Token.IsOfType(PredefinedTokenAndDataTypeNames.AdditionOperator))
                    {
                        binaryOperator = BinaryOperatorType.Addition;
                    }
                    else if (operatorToken.Token.IsOfType(PredefinedTokenAndDataTypeNames.SubstractionOperator))
                    {
                        binaryOperator = BinaryOperatorType.Subtraction;
                    }
                    else
                    {
                        return foundLeftExpression;
                    }

                    ExpressionParserAndInterpreterResult leftExpressionResult = result;
                    ExpressionParserAndInterpreterResult rightExpressionResult = new();

                    bool foundRightExpression
                         = await ParseMultiplicativeExpressionAsync(
                             culture,
                             operatorToken.Next,
                             variableService,
                             rightExpressionResult,
                             cancellationToken);

                    if (foundRightExpression)
                    {
                        result.NextTokenToContinueWith = rightExpressionResult.NextTokenToContinueWith;

                        result.ParsedExpression
                            = new BinaryOperatorExpression(
                                leftExpressionResult.ParsedExpression!,
                                binaryOperator,
                                rightExpressionResult.ParsedExpression!);

                        result.ResultedData
                            = ParserAndInterpreterService.ArithmeticAndRelationOperationService.PerformAlgebraOperation(
                                leftExpressionResult.ResultedData,
                                binaryOperator,
                                rightExpressionResult.ResultedData);

                        operatorToken = rightExpressionResult.NextTokenToContinueWith.SkipNextWordTokens();
                    }
                    else
                    {
                        return foundLeftExpression;
                    }
                }
            }

            return foundLeftExpression;
        }

        /// <summary>
        /// Parse expression that contains multiply, division and modulus symbols.
        /// 
        /// Corresponding grammar :
        ///     Primary_Expression (('*' | '/') Primary_Expression)*
        /// </summary>
        private async Task<bool> ParseMultiplicativeExpressionAsync(
            string culture,
            LinkedToken? currentToken,
            IVariableService variableService,
            ExpressionParserAndInterpreterResult result,
            CancellationToken cancellationToken)
        {
            bool foundLeftExpression
                = await ParserAndInterpreterService.TryParseAndInterpretExpressionAsync(
                    PredefinedExpressionParserNames.PrimitiveExpression,
                    culture,
                    currentToken,
                    variableService,
                    result,
                    cancellationToken);

            if (foundLeftExpression)
            {
                LinkedToken? operatorToken = result.NextTokenToContinueWith.SkipNextWordTokens();
                while (operatorToken is not null)
                {
                    BinaryOperatorType binaryOperator;
                    LinkedToken? expressionStartToken = operatorToken.Next;
                    if (operatorToken.Token.IsOfType(PredefinedTokenAndDataTypeNames.MultiplicationOperator))
                    {
                        binaryOperator = BinaryOperatorType.Multiply;
                    }
                    else if (operatorToken.Token.IsOfType(PredefinedTokenAndDataTypeNames.DivisionOperator))
                    {
                        binaryOperator = BinaryOperatorType.Division;
                    }
                    else if (operatorToken.Token.IsOfType(PredefinedTokenAndDataTypeNames.LeftParenth))
                    {
                        binaryOperator = BinaryOperatorType.Multiply;
                        expressionStartToken = operatorToken;
                    }
                    else if (operatorToken.Token.IsOfType(PredefinedTokenAndDataTypeNames.Numeric))
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
                        return foundLeftExpression;
                    }

                    ExpressionParserAndInterpreterResult leftExpressionResult = result;
                    ExpressionParserAndInterpreterResult rightExpressionResult = new();

                    bool foundRightExpression
                        = await ParserAndInterpreterService.TryParseAndInterpretExpressionAsync(
                            PredefinedExpressionParserNames.PrimitiveExpression,
                            culture,
                            expressionStartToken,
                            variableService,
                            rightExpressionResult,
                            cancellationToken);

                    if (foundRightExpression)
                    {
                        result.NextTokenToContinueWith = rightExpressionResult.NextTokenToContinueWith;

                        result.ParsedExpression
                            = new BinaryOperatorExpression(
                                leftExpressionResult.ParsedExpression!,
                                binaryOperator,
                                rightExpressionResult.ParsedExpression!);

                        result.ResultedData
                            = ParserAndInterpreterService.ArithmeticAndRelationOperationService.PerformAlgebraOperation(
                                leftExpressionResult.ResultedData,
                                binaryOperator,
                                rightExpressionResult.ResultedData);

                        operatorToken = rightExpressionResult.NextTokenToContinueWith.SkipNextWordTokens();
                    }
                    else
                    {
                        return foundLeftExpression;
                    }
                }
            }

            return foundLeftExpression;
        }
    }
}
