using System.Collections.Generic;
using NotepadBasedCalculator.Api;
using Xunit;

namespace NotepadBasedCalculator.Core.Tests
{
    public class LexerTests
    {
        [Fact]
        public void TokenizeEmpty()
        {
            IReadOnlyList<LineInfo> lines = Analyze(string.Empty);
            Assert.Equal(1, lines.Count);
            Assert.Equal(0, lines[0].TokenCount);
        }

        [Fact]
        public void TokenizeWord()
        {
            IReadOnlyList<LineInfo> lines = Analyze(" a b c ");
            Assert.Equal(1, lines.Count);
            Assert.Equal(7, lines[0].TokenCount);
            Assert.Equal(TokenType.Whitespace, lines[0].Tokens[0].Type);
            Assert.Equal(TokenType.Word, lines[0].Tokens[1].Type);
            Assert.Equal(TokenType.Whitespace, lines[0].Tokens[2].Type);
            Assert.Equal(TokenType.Word, lines[0].Tokens[3].Type);
            Assert.Equal(TokenType.Whitespace, lines[0].Tokens[4].Type);
            Assert.Equal(TokenType.Word, lines[0].Tokens[5].Type);
            Assert.Equal(TokenType.Whitespace, lines[0].Tokens[6].Type);

            lines = Analyze("  abæçØ ");
            Assert.Equal(1, lines.Count);
            Assert.Equal(3, lines[0].Tokens.Count);
            Assert.Equal(TokenType.Whitespace, lines[0].Tokens[0].Type);
            Assert.Equal(TokenType.Word, lines[0].Tokens[1].Type);
            Assert.Equal(2, lines[0].Tokens[1].StartIndex);
            Assert.Equal(5, lines[0].Tokens[1].Length);
            Assert.Equal(TokenType.Whitespace, lines[0].Tokens[2].Type);
            Assert.Equal(7, lines[0].Tokens[2].StartIndex);

            Assert.True(lines[0].Tokens[1].IsTokenTextEqualTo("abæçØ", System.StringComparison.Ordinal));
            Assert.False(lines[0].Tokens[0].IsTokenTextEqualTo("abæçØ", System.StringComparison.Ordinal));
            Assert.False(lines[0].Tokens[1].IsTokenTextEqualTo("a", System.StringComparison.Ordinal));
            Assert.False(lines[0].Tokens[1].IsTokenTextEqualTo("bæçØ", System.StringComparison.Ordinal));
            Assert.False(lines[0].Tokens[1].IsTokenTextEqualTo("baæçØ", System.StringComparison.Ordinal));
        }

        [Fact]
        public void TokenizeNumber()
        {
            IReadOnlyList<LineInfo> lines = Analyze(" 1 2 3 ");
            Assert.Equal(1, lines.Count);
            Assert.Equal(7, lines[0].Tokens.Count);
            Assert.Equal(TokenType.Whitespace, lines[0].Tokens[0].Type);
            Assert.Equal(TokenType.Number, lines[0].Tokens[1].Type);
            Assert.Equal(TokenType.Whitespace, lines[0].Tokens[2].Type);
            Assert.Equal(TokenType.Number, lines[0].Tokens[3].Type);
            Assert.Equal(TokenType.Whitespace, lines[0].Tokens[4].Type);
            Assert.Equal(TokenType.Number, lines[0].Tokens[5].Type);
            Assert.Equal(TokenType.Whitespace, lines[0].Tokens[6].Type);

            lines = Analyze(" 12 ");
            Assert.Equal(1, lines.Count);
            Assert.Equal(3, lines[0].Tokens.Count);
            Assert.Equal(TokenType.Whitespace, lines[0].Tokens[0].Type);
            Assert.Equal(TokenType.Number, lines[0].Tokens[1].Type);
            Assert.Equal(1, lines[0].Tokens[1].StartIndex);
            Assert.Equal(2, lines[0].Tokens[1].Length);
            Assert.Equal(TokenType.Whitespace, lines[0].Tokens[2].Type);
            Assert.Equal(3, lines[0].Tokens[2].StartIndex);
        }

        [Fact]
        public void TokenizeWhitespace()
        {
            IReadOnlyList<LineInfo> lines = Analyze(" ");
            Assert.Equal(1, lines.Count);
            Assert.Equal(1, lines[0].Tokens.Count);
            Assert.Equal(TokenType.Whitespace, lines[0].Tokens[0].Type);
            Assert.Equal(0, lines[0].Tokens[0].StartIndex);
            Assert.Equal(1, lines[0].Tokens[0].Length);

            lines = Analyze("  ");
            Assert.Equal(1, lines.Count);
            Assert.Equal(1, lines[0].Tokens.Count);
            Assert.Equal(TokenType.Whitespace, lines[0].Tokens[0].Type);
            Assert.Equal(0, lines[0].Tokens[0].StartIndex);
            Assert.Equal(2, lines[0].Tokens[0].Length);

            lines = Analyze("   ");
            Assert.Equal(1, lines.Count);
            Assert.Equal(1, lines[0].Tokens.Count);
            Assert.Equal(TokenType.Whitespace, lines[0].Tokens[0].Type);
            Assert.Equal(0, lines[0].Tokens[0].StartIndex);
            Assert.Equal(3, lines[0].Tokens[0].Length);

            lines = Analyze(" \t ");
            Assert.Equal(1, lines.Count);
            Assert.Equal(1, lines[0].Tokens.Count);
            Assert.Equal(TokenType.Whitespace, lines[0].Tokens[0].Type);
            Assert.Equal(0, lines[0].Tokens[0].StartIndex);
            Assert.Equal(3, lines[0].Tokens[0].Length);
        }

        [Fact]
        public void TokenizePunctuationAndSymbols()
        {
            IReadOnlyList<LineInfo> lines = Analyze("!@#$%^&*()+_=`~[]{}\\|;:'\",<.>/?");
            Assert.Equal(1, lines.Count);
            Assert.Equal(31, lines[0].Tokens.Count);
            for (int i = 0; i < lines[0].Tokens.Count; i++)
            {
                Assert.Equal(TokenType.SymbolOrPunctuation, lines[0].Tokens[i].Type);
                Assert.Equal(1, lines[0].Tokens[i].Length);
            }

            lines = Analyze("π÷–−×·⋅¿¾½¼»º¹¸·¶µ´³²±°¯®­¬«ª©¨§¦¥¤£¢¡~}|{");
            Assert.Equal(1, lines.Count);
            Assert.Equal(42, lines[0].Tokens.Count);
            for (int i = 0; i < lines[0].Tokens.Count; i++)
            {
                Assert.Equal(TokenType.SymbolOrPunctuation, lines[0].Tokens[i].Type);
                Assert.Equal(1, lines[0].Tokens[i].Length);
            }
        }

        [Fact]
        public void TokenizeMultipleLines()
        {
            IReadOnlyList<LineInfo> lines = Analyze("    \r\n\r\n\nabc\n \r  ");
            Assert.Equal(6, lines.Count);
            Assert.Equal(1, lines[0].TokenCount);
            Assert.Equal(TokenType.Whitespace, lines[0].Tokens[0].Type);
            Assert.Equal(0, lines[1].TokenCount);
            Assert.Equal(0, lines[2].TokenCount);
            Assert.Equal(TokenType.Word, lines[3].Tokens[0].Type);
            Assert.Equal(1, lines[4].TokenCount);
            Assert.Equal(TokenType.Whitespace, lines[4].Tokens[0].Type);
            Assert.Equal(1, lines[5].TokenCount);
            Assert.Equal(TokenType.Whitespace, lines[5].Tokens[0].Type);
        }

        private static IReadOnlyList<LineInfo> Analyze(string input)
        {
            var lines = new List<LineInfo>();
            int lineCount = 0;
            var lexer = new Lexer();
            LinkedToken lineTokens = lexer.GetLineTokens(input);

            do
            {
                lineCount++;
                var tokens = new List<Token>();
                if (lineTokens is not null)
                {
                    int tokenEndIndexWithCarriageReturn = 0;
                    while (lineTokens is not null)
                    {
                        if (lineTokens.Token.Type != TokenType.NewLine)
                        {
                            tokens.Add(lineTokens.Token);
                        }
                        tokenEndIndexWithCarriageReturn = lineTokens.TokenEndIndexWithCarriageReturn;
                        lineTokens = lineTokens.Next;
                    }

                    lineTokens = lexer.GetLineTokens(input, startIndex: tokenEndIndexWithCarriageReturn);
                }

                var lineInfo
                    = new LineInfo
                    {
                        LineNumber = lineCount,
                        TokenCount = tokens.Count,
                        Tokens = tokens
                    };
                lines.Add(lineInfo);
            }
            while (lineTokens is not null);

            return lines;
        }

        private class LineInfo
        {
            internal IReadOnlyList<Token> Tokens { get; set; }

            internal int TokenCount { get; set; }

            internal int LineNumber { get; set; }
        }
    }
}
