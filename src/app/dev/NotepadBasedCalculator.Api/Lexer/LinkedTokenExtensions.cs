namespace NotepadBasedCalculator.Api
{
    public static class LinkedTokenExtensions
    {
        public static LinkedToken? GetTokenAfter(this LinkedToken? sourceToken, LinkedToken tokenToJump)
        {
            while (sourceToken is not null && sourceToken.Token.EndInLine < tokenToJump.Token.EndInLine)
            {
                sourceToken = sourceToken.Next;
            }

            return sourceToken;
        }

        public static LinkedToken? SkipToLastToken(this LinkedToken? currentToken)
        {
            while (currentToken?.Next is not null)
            {
                currentToken = currentToken.Next;
            }

            return currentToken;
        }

        public static LinkedToken? SkipNextWordTokens(this LinkedToken? currentToken)
        {
            while (currentToken is not null && currentToken.Token.IsOfType(PredefinedTokenAndDataTypeNames.Word))
            {
                currentToken = currentToken.Next;
            }

            return currentToken;
        }

        public static bool SkipToken(
            this LinkedToken? currentToken,
            string expectedTokenType,
            bool skipWordsToken,
            out LinkedToken? nextToken)
        {
            LinkedToken? backupToken = currentToken;
            if (skipWordsToken)
            {
                currentToken = currentToken.SkipNextWordTokens();
            }

            if (currentToken is null || currentToken.Token.IsNotOfType(expectedTokenType))
            {
                nextToken = backupToken;
                return false;
            }

            nextToken = currentToken.Next;
            return true;
        }

        public static bool JumpToNextTokenOfType(
            this LinkedToken? currentToken,
            string tokenType,
            out LinkedToken? nextToken)
        {
            return currentToken.JumpToNextTokenOfType(tokenType, string.Empty, out nextToken);
        }

        public static bool JumpToNextTokenOfType(
            this LinkedToken? currentToken,
            string tokenType,
            string tokenText,
            out LinkedToken? nextToken)
        {
            while (currentToken is not null)
            {
                if (string.IsNullOrEmpty(tokenText))
                {
                    if (currentToken.Token.IsOfType(tokenType))
                    {
                        nextToken = currentToken;
                        return true;
                    }
                }
                else
                {
                    if (currentToken.Token.Is(tokenType, tokenText))
                    {
                        nextToken = currentToken;
                        return true;
                    }
                }

                currentToken = currentToken.Next;
            }

            nextToken = null;
            return false;
        }
    }
}
