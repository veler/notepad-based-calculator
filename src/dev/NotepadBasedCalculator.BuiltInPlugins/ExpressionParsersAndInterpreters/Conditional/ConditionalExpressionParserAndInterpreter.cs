using System.Runtime.CompilerServices;
using NotepadBasedCalculator.BuiltInPlugins.Data.Definition;

namespace NotepadBasedCalculator.BuiltInPlugins.ExpressionParsersAndInterpreters.Conditional
{
    [Export(typeof(IExpressionParserAndInterpreter))]
    [Name(PredefinedExpressionParserNames.ConditionalExpression)]
    [Culture(SupportedCultures.Any)]
    internal sealed class ConditionalExpressionParserAndInterpreter : IExpressionParserAndInterpreter
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
                ParseEqualityAndRelationalExpression(
                    culture,
                    currentToken,
                    variableService,
                    result,
                    cancellationToken);
        }

        /// <summary>
        /// Parse expression that contains equality symbols.
        /// 
        /// Corresponding grammar :
        ///     NumericalCalculus_Expression (('==' | '!=' | '<' | '>' | '<=' | '>=') NumericalCalculus_Expression)
        /// </summary>
        private async Task<bool> ParseEqualityAndRelationalExpression(
            string culture,
            LinkedToken? currentToken,
            IVariableService variableService,
            ExpressionParserAndInterpreterResult result,
            CancellationToken cancellationToken)
        {
            bool foundLeftExpression
                = await ParserAndInterpreterService.TryParseAndInterpretExpressionAsync(
                    PredefinedExpressionParserNames.NumericalExpression,
                    culture,
                    currentToken,
                    variableService,
                    result,
                    cancellationToken);

            if (foundLeftExpression)
            {
                LinkedToken? operatorToken = result.NextTokenToContinueWith.SkipNextWordTokens();

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
                        return foundLeftExpression;
                    }

                    ExpressionParserAndInterpreterResult leftExpressionResult = result;
                    ExpressionParserAndInterpreterResult rightExpressionResult = new();

                    bool foundRightExpression
                        = await ParserAndInterpreterService.TryParseAndInterpretExpressionAsync(
                            PredefinedExpressionParserNames.NumericalExpression,
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
                            = PerformBinaryOperation(
                                leftExpressionResult.ResultedData,
                                binaryOperator,
                                rightExpressionResult.ResultedData);
                    }
                    else
                    {
                        return foundLeftExpression;
                    }
                }
            }

            return foundLeftExpression;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IData? PerformBinaryOperation(IData? leftData, BinaryOperatorType binaryOperatorType, IData? rightData)
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

            bool result;

            switch (binaryOperatorType)
            {
                case BinaryOperatorType.Equality:
                    result
                        = leftNumericData.ToStandardUnit().NumericValue
                        == rightNumericData.ToStandardUnit().NumericValue;
                    break;

                case BinaryOperatorType.NoEquality:
                    result
                        = leftNumericData.ToStandardUnit().NumericValue
                        != rightNumericData.ToStandardUnit().NumericValue;
                    break;

                case BinaryOperatorType.LessThan:
                    result
                        = leftNumericData.ToStandardUnit().NumericValue
                        < rightNumericData.ToStandardUnit().NumericValue;
                    break;

                case BinaryOperatorType.LessThanOrEqualTo:
                    result
                        = leftNumericData.ToStandardUnit().NumericValue
                        <= rightNumericData.ToStandardUnit().NumericValue;
                    break;

                case BinaryOperatorType.GreaterThan:
                    result
                        = leftNumericData.ToStandardUnit().NumericValue
                        > rightNumericData.ToStandardUnit().NumericValue;
                    break;

                case BinaryOperatorType.GreaterThanOrEqualTo:
                    result
                        = leftNumericData.ToStandardUnit().NumericValue
                        >= rightNumericData.ToStandardUnit().NumericValue;
                    break;

                default:
                    ThrowHelper.ThrowNotSupportedException();
                    return null;
            }

            return new BooleanData(
                leftData.LineTextIncludingLineBreak,
                leftData.StartInLine,
                rightData.EndInLine,
                result);
        }
    }
}
