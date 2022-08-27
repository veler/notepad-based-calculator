using System.Runtime.CompilerServices;

namespace NotepadBasedCalculator.BuiltInPlugins.ExpressionParsersAndInterpreters.Numerical
{
    [Export(typeof(IExpressionParserAndInterpreter))]
    [Name(PredefinedExpressionParserNames.NumericalExpression)]
    [Culture(SupportedCultures.Any)]
    [Order(int.MaxValue - 1)]
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
                            = PerformAlgebraOperation(
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
                = await ParserPrimaryExpressionAsync(
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
                        return foundLeftExpression;
                    }

                    ExpressionParserAndInterpreterResult leftExpressionResult = result;
                    ExpressionParserAndInterpreterResult rightExpressionResult = new();

                    bool foundRightExpression
                         = await ParserPrimaryExpressionAsync(
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
                            = PerformAlgebraOperation(
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
        /// Parse an expression that can be either a primitive data, a variable reference or an expression between parenthesis.
        /// 
        /// Corresponding grammar :
        ///     Primitive_Value
        ///     | Identifier
        ///     | '(' Expression ')'
        /// </summary>
        private async Task<bool> ParserPrimaryExpressionAsync(
            string culture,
            LinkedToken? currentToken,
            IVariableService variableService,
            ExpressionParserAndInterpreterResult result,
            CancellationToken cancellationToken)
        {
            currentToken = currentToken.SkipNextWordTokens();
            if (currentToken is not null)
            {
                // Detect Numbers, Percentage, Dates...etc.
                if (currentToken.Token is IData data)
                {
                    var expression = new DataExpression(currentToken, currentToken, data);
                    result.NextTokenToContinueWith = currentToken.Next;
                    result.ParsedExpression = expression;
                    result.ResultedData = expression.Data;
                    return true;
                }

                // Detect variable reference
                if (currentToken.Token.Is(PredefinedTokenAndDataTypeNames.VariableReference))
                {
                    var expression = new VariableReferenceExpression(currentToken);
                    result.NextTokenToContinueWith = currentToken.Next;
                    result.ParsedExpression = expression;
                    result.ResultedData = variableService.GetVariableValue(expression.VariableName);
                    return true;
                }

                // Detect expression between parenthesis.
                LinkedToken leftParenthToken = currentToken;
                if (DiscardLeftParenth(leftParenthToken, out LinkedToken? nextToken))
                {
                    bool foundExpression
                         = await ParserAndInterpreterService.TryParseAndInterpretExpressionAsync(
                             culture,
                            nextToken,
                            variableService,
                            result,
                            cancellationToken);

                    LinkedToken? rightParenthToken = result.NextTokenToContinueWith?.SkipNextWordTokens();
                    if (foundExpression
                        && DiscardRightParenth(rightParenthToken, out nextToken)
                        && rightParenthToken is not null)
                    {
                        result.NextTokenToContinueWith = nextToken;
                        result.ParsedExpression = new GroupExpression(leftParenthToken, rightParenthToken, result.ParsedExpression!);
                        // no need to update the data in the `result`. It should already be set by `ParserAndInterpreterService`.
                        return true;
                    }
                }
            }

            result.NextTokenToContinueWith = null;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool DiscardLeftParenth(LinkedToken? currentToken, out LinkedToken? nextToken)
        {
            return currentToken.SkipToken(
                PredefinedTokenAndDataTypeNames.LeftParenth,
                skipWordsToken: true,
                out nextToken);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool DiscardRightParenth(LinkedToken? currentToken, out LinkedToken? nextToken)
        {
            return currentToken.SkipToken(
                PredefinedTokenAndDataTypeNames.RightParenth,
                skipWordsToken: true,
                out nextToken);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IData? PerformAlgebraOperation(IData? leftData, BinaryOperatorType binaryOperatorType, IData? rightData)
        {
            if (leftData is null || rightData is null)
            {
                return leftData;
            }

            if (leftData is not INumericData leftNumericData
                || rightData is not INumericData rightNumericData)
            {
                return null;
            }

            bool tryToConvertLeftData = true;
            if (leftData is IConvertibleNumericData leftConvertibleNumericData)
            {
                if (leftConvertibleNumericData.CanConvertFrom(rightNumericData))
                {
                    INumericData? newRightNumericData = leftConvertibleNumericData.ConvertFrom(rightNumericData);
                    if (newRightNumericData is not null)
                    {
                        rightNumericData = newRightNumericData;
                        tryToConvertLeftData = false;
                    }
                }
            }

            if (tryToConvertLeftData && rightData is IConvertibleNumericData rightConvertibleNumericData)
            {
                if (rightConvertibleNumericData.CanConvertFrom(leftNumericData))
                {
                    INumericData? newLeftNumericData = rightConvertibleNumericData.ConvertFrom(leftNumericData);
                    if (newLeftNumericData is null)
                    {
                        return null;
                    }
                    leftNumericData = newLeftNumericData;
                }
                else
                {
                    return null;
                }
            }

            double result;
            switch (binaryOperatorType)
            {
                case BinaryOperatorType.Addition:
                    result
                        = leftNumericData.ToStandardUnit().NumericValue
                        + rightNumericData.ToStandardUnit().GetNumericValueToRelativeTo(leftNumericData);
                    break;

                case BinaryOperatorType.Subtraction:
                    result
                        = leftNumericData.ToStandardUnit().NumericValue
                        - rightNumericData.ToStandardUnit().GetNumericValueToRelativeTo(leftNumericData);
                    break;

                case BinaryOperatorType.Multiply:
                    result
                        = leftNumericData.ToStandardUnit().NumericValue
                        * rightNumericData.ToStandardUnit().NumericValue;
                    break;

                case BinaryOperatorType.Division:
                    double divisor = rightNumericData.ToStandardUnit().NumericValue;
                    if (divisor == 0)
                    {
                        result = double.PositiveInfinity;
                    }
                    else
                    {
                        result
                            = leftNumericData.ToStandardUnit().NumericValue
                            / divisor;
                    }
                    break;

                default:
                    ThrowHelper.ThrowNotSupportedException();
                    return null;
            }

            return leftNumericData.FromStandardUnit(result).MergeDataLocations(rightNumericData);
        }
    }
}
