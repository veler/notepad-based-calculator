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
            var lexer = new Lexer();
            IReadOnlyList<IReadOnlyList<Token>> tokens = lexer.Tokenize(string.Empty);
            Assert.Equal(0, tokens.Count);
        }

        [Fact]
        public void TokenizeWord()
        {
            var lexer = new Lexer();
            IReadOnlyList<IReadOnlyList<Token>> tokens = lexer.Tokenize(" a b c ");
            Assert.Equal(1, tokens.Count);
            Assert.Equal(7, tokens[0].Count);
            Assert.Equal(TokenType.Whitespace, tokens[0][0].Type);
            Assert.Equal(TokenType.Word, tokens[0][1].Type);
            Assert.Equal(TokenType.Whitespace, tokens[0][2].Type);
            Assert.Equal(TokenType.Word, tokens[0][3].Type);
            Assert.Equal(TokenType.Whitespace, tokens[0][4].Type);
            Assert.Equal(TokenType.Word, tokens[0][5].Type);
            Assert.Equal(TokenType.Whitespace, tokens[0][6].Type);

            tokens = lexer.Tokenize("  abæçØ ");
            Assert.Equal(1, tokens.Count);
            Assert.Equal(3, tokens[0].Count);
            Assert.Equal(TokenType.Whitespace, tokens[0][0].Type);
            Assert.Equal(TokenType.Word, tokens[0][1].Type);
            Assert.Equal(2, tokens[0][1].StartIndex);
            Assert.Equal(5, tokens[0][1].Length);
            Assert.Equal(TokenType.Whitespace, tokens[0][2].Type);
            Assert.Equal(7, tokens[0][2].StartIndex);

            Assert.True(tokens[0][1].IsTokenTextEqualTo("abæçØ", System.StringComparison.Ordinal));
            Assert.False(tokens[0][0].IsTokenTextEqualTo("abæçØ", System.StringComparison.Ordinal));
            Assert.False(tokens[0][1].IsTokenTextEqualTo("a", System.StringComparison.Ordinal));
            Assert.False(tokens[0][1].IsTokenTextEqualTo("bæçØ", System.StringComparison.Ordinal));
            Assert.False(tokens[0][1].IsTokenTextEqualTo("baæçØ", System.StringComparison.Ordinal));
        }

        [Fact]
        public void TokenizeNumber()
        {
            var lexer = new Lexer();
            IReadOnlyList<IReadOnlyList<Token>> tokens = lexer.Tokenize(" 1 2 3 ");
            Assert.Equal(1, tokens.Count);
            Assert.Equal(7, tokens[0].Count);
            Assert.Equal(TokenType.Whitespace, tokens[0][0].Type);
            Assert.Equal(TokenType.Number, tokens[0][1].Type);
            Assert.Equal(TokenType.Whitespace, tokens[0][2].Type);
            Assert.Equal(TokenType.Number, tokens[0][3].Type);
            Assert.Equal(TokenType.Whitespace, tokens[0][4].Type);
            Assert.Equal(TokenType.Number, tokens[0][5].Type);
            Assert.Equal(TokenType.Whitespace, tokens[0][6].Type);

            tokens = lexer.Tokenize(" 12 ");
            Assert.Equal(1, tokens.Count);
            Assert.Equal(3, tokens[0].Count);
            Assert.Equal(TokenType.Whitespace, tokens[0][0].Type);
            Assert.Equal(TokenType.Number, tokens[0][1].Type);
            Assert.Equal(1, tokens[0][1].StartIndex);
            Assert.Equal(2, tokens[0][1].Length);
            Assert.Equal(TokenType.Whitespace, tokens[0][2].Type);
            Assert.Equal(3, tokens[0][2].StartIndex);
        }

        [Fact]
        public void TokenizeWhitespace()
        {
            var lexer = new Lexer();
            IReadOnlyList<IReadOnlyList<Token>> tokens = lexer.Tokenize(" ");
            Assert.Equal(1, tokens.Count);
            Assert.Equal(1, tokens[0].Count);
            Assert.Equal(TokenType.Whitespace, tokens[0][0].Type);
            Assert.Equal(0, tokens[0][0].StartIndex);
            Assert.Equal(1, tokens[0][0].Length);

            tokens = lexer.Tokenize("  ");
            Assert.Equal(1, tokens.Count);
            Assert.Equal(1, tokens[0].Count);
            Assert.Equal(TokenType.Whitespace, tokens[0][0].Type);
            Assert.Equal(0, tokens[0][0].StartIndex);
            Assert.Equal(2, tokens[0][0].Length);

            tokens = lexer.Tokenize("   ");
            Assert.Equal(1, tokens.Count);
            Assert.Equal(1, tokens[0].Count);
            Assert.Equal(TokenType.Whitespace, tokens[0][0].Type);
            Assert.Equal(0, tokens[0][0].StartIndex);
            Assert.Equal(3, tokens[0][0].Length);
        }

        [Fact]
        public void TokenizePunctuationAndSymbols()
        {
            var lexer = new Lexer();
            IReadOnlyList<IReadOnlyList<Token>> tokens = lexer.Tokenize("!@#$%^&*()+_=`~[]{}\\|;:'\",<.>/?");
            Assert.Equal(1, tokens.Count);
            Assert.Equal(31, tokens[0].Count);
            for (int i = 0; i < tokens[0].Count; i++)
            {
                Assert.Equal(TokenType.SymbolOrPunctuation, tokens[0][i].Type);
                Assert.Equal(1, tokens[0][i].Length);
            }

            tokens = lexer.Tokenize("π÷–−×·⋅¿¾½¼»º¹¸·¶µ´³²±°¯®­¬«ª©¨§¦¥¤£¢¡~}|{");
            Assert.Equal(1, tokens.Count);
            Assert.Equal(42, tokens[0].Count);
            for (int i = 0; i < tokens[0].Count; i++)
            {
                Assert.Equal(TokenType.SymbolOrPunctuation, tokens[0][i].Type);
                Assert.Equal(1, tokens[0][i].Length);
            }
        }

        [Fact]
        public void TokenizeMultipleLines()
        {
            var lexer = new Lexer();
            IReadOnlyList<IReadOnlyList<Token>> tokens = lexer.Tokenize("    \r\nabc\n   ");
            Assert.Equal(3, tokens.Count);
            Assert.Equal(1, tokens[0].Count);
            Assert.Equal(TokenType.Whitespace, tokens[0][0].Type);
            Assert.Equal(1, tokens[1].Count);
            Assert.Equal(TokenType.Word, tokens[1][0].Type);
            Assert.Equal(1, tokens[2].Count);
            Assert.Equal(TokenType.Whitespace, tokens[2][0].Type);
        }
    }
}
