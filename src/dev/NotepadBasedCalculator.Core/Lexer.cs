using NotepadBasedCalculator.Api;

namespace NotepadBasedCalculator.Core
{
    internal sealed class Lexer
    {
        internal IReadOnlyList<IReadOnlyList<Token>> Tokenize(string? input)
        {
            var tokens = new List<List<Token>>();

            if (!string.IsNullOrEmpty(input))
            {
                int i = 0;
                var tokensInLine = new List<Token>();
                while (i < input!.Length)
                {
                    Token token = DetectToken(input, i);
                    if (token.Type == TokenType.NewLine)
                    {
                        tokens.Add(tokensInLine);
                        tokensInLine = new();
                    }
                    else
                    {
                        tokensInLine.Add(token);
                    }

                    i = token.EndIndex;
                }

                tokens.Add(tokensInLine);
            }

            // TODO: Potential optimization => could we return directly a LinkedToken?
            return tokens;
        }

        private static Token DetectToken(string input, int startIndex)
        {
            char startChar = input[startIndex];
            int endIndex = startIndex + 1;

            TokenType tokenType = DetectTokenType(startChar);

            if (input.Length > startIndex)
            {
                int nextCharIndex;
                switch (tokenType)
                {
                    case TokenType.Word:
                    case TokenType.Number:
                    case TokenType.Whitespace:
                    case TokenType.NewLine:
                        nextCharIndex = GetEndPositionOfRepeatedTokenType(input, startIndex, tokenType);
                        if (nextCharIndex > startIndex + 1)
                        {
                            endIndex = nextCharIndex;
                        }
                        break;

                    default:
                        break;
                }
            }

            return new Token(tokenType, startIndex, endIndex);
        }

        private static int GetEndPositionOfRepeatedTokenType(string input, int startIndex, TokenType tokenType)
        {
            int nextCharIndex = startIndex;
            do
            {
                nextCharIndex++;
            } while (input.Length > nextCharIndex && DetectTokenType(input[nextCharIndex]) == tokenType);
            return nextCharIndex;
        }

        private static TokenType DetectTokenType(char c)
        {
            if (c == '\r' || c == '\n')
            {
                return TokenType.NewLine;
            }

            if (char.IsDigit(c))
            {
                return TokenType.Number;
            }

            if (char.IsWhiteSpace(c))
            {
                return TokenType.Whitespace;
            }

            if (char.IsPunctuation(c)
                || char.IsSymbol(c)
                || c == 'π'
                || c == '¾'
                || c == '½'
                || c == '¼'
                || c == 'º'
                || c == '¹'
                || c == '²'
                || c == '³'
                || c == 'µ'
                || c == '­'
                || c == 'ª')
            {
                return TokenType.SymbolOrPunctuation;
            }

            if (char.IsLetter(c))
            {
                return TokenType.Word;
            }

            return TokenType.UnsupportedCharacter;
        }
    }
}
