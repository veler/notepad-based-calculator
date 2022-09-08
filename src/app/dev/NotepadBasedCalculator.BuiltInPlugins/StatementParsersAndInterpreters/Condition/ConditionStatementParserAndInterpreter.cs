namespace NotepadBasedCalculator.BuiltInPlugins.StatementParsersAndInterpreters.Condition
{
    [Export(typeof(IStatementParserAndInterpreter))]
    [Culture(SupportedCultures.English)]
    internal sealed class ConditionStatementParserAndInterpreter : IStatementParserAndInterpreter
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
            if (!currentToken.SkipToken(
                PredefinedTokenAndDataTypeNames.IfIdentifier,
                skipWordsToken: true,
                out LinkedToken? tokenAfterIfIdentifier))
            {
                return false;
            }

            var expressionResult = new ExpressionParserAndInterpreterResult();

            bool expressionFound
                = await ParserAndInterpreterService.TryParseAndInterpretExpressionAsync(
                    PredefinedExpressionParserNames.ConditionalExpression,
                    culture,
                    tokenAfterIfIdentifier,
                    PredefinedTokenAndDataTypeNames.ThenIdentifier,
                    string.Empty,
                    variableService,
                    expressionResult,
                    cancellationToken);

            if (expressionFound && expressionResult.ResultedData is BooleanData resultedBooleanData)
            {
                var statementResult = new StatementParserAndInterpreterResult();
                if (resultedBooleanData.Value)
                {
                    // Interpret statement after `then` identified.
                    if (expressionResult.NextTokenToContinueWith.SkipToken(
                        PredefinedTokenAndDataTypeNames.ThenIdentifier,
                        skipWordsToken: true,
                        out LinkedToken? tokenAfterThenIdentifier))
                    {
                        bool statementFound
                            = await ParserAndInterpreterService.TryParseAndInterpretStatementAsync(
                                culture,
                                tokenAfterThenIdentifier!,
                                PredefinedTokenAndDataTypeNames.ElseIdentifier,
                                string.Empty,
                                variableService,
                                statementResult,
                                cancellationToken);

                        result.ParsedStatement
                            = new ConditionStatement(
                                currentToken,
                                currentToken.SkipToLastToken()!,
                                expressionResult.ParsedExpression!,
                                statementFound ? statementResult.ResultedData : null);
                        result.ResultedData = statementResult?.ResultedData;
                        return true;
                    }
                }
                else
                {
                    // Either interpret the statement after `else` identifier, or opt-out.
                    if (expressionResult.NextTokenToContinueWith.JumpToNextTokenOfType(
                        PredefinedTokenAndDataTypeNames.ElseIdentifier,
                        out LinkedToken? elseIdentifier))
                    {
                        await ParserAndInterpreterService.TryParseAndInterpretStatementAsync(
                            culture,
                            elseIdentifier!.Next,
                            PredefinedTokenAndDataTypeNames.ElseIdentifier,
                            string.Empty,
                            variableService,
                            statementResult,
                            cancellationToken);
                    }

                    result.ParsedStatement
                        = new ConditionStatement(
                            currentToken,
                            currentToken.SkipToLastToken()!,
                            expressionResult.ParsedExpression!,
                            statementResult.ResultedData);
                    result.ResultedData = statementResult?.ResultedData;
                    return true;
                }
            }

            return false;
        }
    }
}
