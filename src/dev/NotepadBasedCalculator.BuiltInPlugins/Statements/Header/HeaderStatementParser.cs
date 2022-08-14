namespace NotepadBasedCalculator.BuiltInPlugins.Statements.Header
{
    [Export(typeof(IStatementParser))]
    [Culture(SupportedCultures.Any)]
    internal sealed class HeaderStatementParser : ParserBase, IStatementParser
    {
        public bool TryParseStatement(string culture, LinkedToken currentToken, out Statement? statement)
        {
            if (currentToken.Token.Is(PredefinedTokenAndDataTypeNames.SymbolOrPunctuation, "#", StringComparison.InvariantCulture))
            {
                LinkedToken? previousToken = currentToken.Previous;
                LinkedToken firstTokenInLine = currentToken;
                while (previousToken is not null)
                {
                    firstTokenInLine = previousToken;
                    if (previousToken.Token.IsNot(PredefinedTokenAndDataTypeNames.Whitespace))
                    {
                        statement = null;
                        return false;
                    }

                    previousToken = previousToken.Previous;
                }

                LinkedToken? nextToken = currentToken.Next;
                LinkedToken lastTokenInLine = currentToken;
                while (nextToken is not null)
                {
                    lastTokenInLine = nextToken;
                    nextToken = nextToken.Next;
                }

                statement = new HeaderStatement(firstTokenInLine, lastTokenInLine);
                return true;
            }

            statement = null;
            return false;
        }
    }
}
