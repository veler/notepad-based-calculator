namespace NotepadBasedCalculator.BuiltInPlugins.StatementParsersAndInterpreters.VariableDeclaration
{
    [Export(typeof(IStatementParserAndInterpreter))]
    [Culture(SupportedCultures.Any)]
    [Order(int.MinValue + 1)]
    internal sealed class VariableDeclarationStatementParserAndInterpreter : IStatementParserAndInterpreter
    {
        [Import]
        public IParserAndInterpreterService ParserAndInterpreterService { get; set; } = null!;

        public async Task<bool> TryParseAndInterpretStatementAsync(
            string culture,
            LinkedToken currentToken,
            IVariableService variableService,
            StatementParserAndInterpreterResult result,
            CancellationToken cancellationToken)
        {
            bool foundEqualSymbol
                = currentToken.JumpToNextTokenOfType(
                    PredefinedTokenAndDataTypeNames.SymbolOrPunctuation,
                    "=",
                    out LinkedToken? equalToken);

            if (!foundEqualSymbol || equalToken is null)
            {
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
                    ExpressionParserAndInterpreterResult assignedExpressionResult = new();
                    bool foundAssignedExpression
                        = await ParserAndInterpreterService.TryParseAndInterpretExpressionAsync(
                            culture,
                            equalToken.Next,
                            variableService,
                            assignedExpressionResult,
                            cancellationToken);

                    if (foundAssignedExpression)
                    {
                        var statement
                            = new VariableDeclarationStatement(
                                variableNameStart,
                                variableNameEnd,
                                variableName,
                                assignedExpressionResult.ParsedExpression!);
                        variableService.SetVariableValue(statement.VariableName, assignedExpressionResult.ResultedData);

                        result.ParsedStatement = statement;
                        result.ResultedData = assignedExpressionResult.ResultedData;
                        return true;
                    }
                }
            }

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
