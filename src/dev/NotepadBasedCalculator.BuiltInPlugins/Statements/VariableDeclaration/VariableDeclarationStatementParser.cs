namespace NotepadBasedCalculator.BuiltInPlugins.Statements.VariableDeclaration
{
    [Export(typeof(IStatementParser))]
    [Culture(SupportedCultures.Any)]
    [Order(int.MinValue + 1)]
    internal sealed class VariableDeclarationStatementParser : ParserBase, IStatementParser
    {
        public bool TryParseStatement(string culture, LinkedToken currentToken, out Statement? statement)
        {
            LinkedToken? equalToken = JumpToNextTokenOfType(currentToken, PredefinedTokenAndDataTypeNames.SymbolOrPunctuation, "=");

            if (equalToken is null)
            {
                statement = null;
                return false;
            }

            LinkedToken variableNameStart;
            LinkedToken? variableNameEnd;

            if (IsAssigneeAnExistingVariable(equalToken))
            {
                variableNameStart = equalToken.Previous!;
                variableNameEnd = equalToken.Previous!;
            }
            else
            {
                LinkedToken? previousToken = equalToken.Previous;
                variableNameStart = equalToken;
                variableNameEnd = previousToken;
                while (previousToken is not null)
                {
                    variableNameStart = previousToken;
                    if (previousToken.Token.IsNot(PredefinedTokenAndDataTypeNames.Word)
                        && previousToken.Token.IsNot(PredefinedTokenAndDataTypeNames.VariableReference))
                    {
                        statement = null;
                        return false;
                    }

                    previousToken = previousToken.Previous;
                }
            }

            if (variableNameEnd is not null)
            {
                string variableName = variableNameStart.Token.GetText(variableNameStart.Token.StartInLine, variableNameEnd.Token.EndInLine);
                if (!string.IsNullOrWhiteSpace(variableName))
                {
                    Expression? assignedValueExpression = ParseExpression(culture, equalToken.Next, out _);
                    if (assignedValueExpression is not null)
                    {
                        statement = new VariableDeclarationStatement(variableNameStart, variableNameEnd, variableName, assignedValueExpression);
                        return true;
                    }
                }
            }

            statement = null;
            return false;
        }

        private static bool IsAssigneeAnExistingVariable(LinkedToken currentToken)
        {
            return currentToken.Previous is not null
                && currentToken.Previous.Token.Is(PredefinedTokenAndDataTypeNames.VariableReference)
                && currentToken.Previous.Previous is null;
        }
    }
}
