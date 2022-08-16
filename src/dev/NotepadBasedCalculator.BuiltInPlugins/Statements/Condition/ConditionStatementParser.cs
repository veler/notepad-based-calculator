using NotepadBasedCalculator.BuiltInPlugins.Statements.NumericalCalculus;

namespace NotepadBasedCalculator.BuiltInPlugins.Statements.Condition
{
    [Export(typeof(IStatementParser))]
    [Culture(SupportedCultures.Any)]
    internal sealed class ConditionStatementParser : ParserBase, IStatementParser
    {
        public bool TryParseStatement(string culture, LinkedToken currentToken, out Statement? statement)
        {
            if (DiscardIf(currentToken, out currentToken))
            {
                LinkedToken ifToken = currentToken.Previous!;

                Expression? expression = ParseExpression(PredefinedExpressionParserNames.ConditionalExpression, culture, currentToken, out LinkedToken? nextToken);

                if (expression is not null)
                {
                    LinkedToken? thenToken = DiscardWords(nextToken);
                    if (thenToken is not null && DiscardThen(thenToken, out _))
                    {
                        // TODO: parse Then and Else.
                        statement = new ConditionStatement(ifToken, thenToken, expression);
                        return true;
                    }
                }
            }
            else
            {
                Expression? expression = ParseExpression(PredefinedExpressionParserNames.ConditionalExpression, culture, currentToken, out LinkedToken? _);
                if (expression is not null
                    && expression is BinaryOperatorExpression binaryOperatorExpression
                    && (binaryOperatorExpression.Operator == BinaryOperatorType.Equality
                        || binaryOperatorExpression.Operator == BinaryOperatorType.NoEquality
                        || binaryOperatorExpression.Operator == BinaryOperatorType.LessThan
                        || binaryOperatorExpression.Operator == BinaryOperatorType.LessThanOrEqualTo
                        || binaryOperatorExpression.Operator == BinaryOperatorType.GreaterThan
                        || binaryOperatorExpression.Operator == BinaryOperatorType.GreaterThanOrEqualTo))
                {
                    statement = new ConditionStatement(expression.FirstToken, expression.LastToken, expression);
                    return true;
                }
            }

            statement = null;
            return false;
        }

        private bool DiscardIf(LinkedToken currentToken, out LinkedToken nextToken)
        {
            return DiscardToken(
                currentToken,
                PredefinedTokenAndDataTypeNames.IfIdentifier,
                ignoreUnknownWords: true,
                out nextToken!)
                && nextToken is not null;
        }

        private bool DiscardThen(LinkedToken? currentToken, out LinkedToken? nextToken)
        {
            return DiscardToken(
                currentToken,
                PredefinedTokenAndDataTypeNames.ThenIdentifier,
                ignoreUnknownWords: true,
                out nextToken!);
        }
    }
}
