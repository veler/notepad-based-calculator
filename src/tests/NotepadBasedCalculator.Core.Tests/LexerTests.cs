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
            IReadOnlyList<Token> tokens = lexer.Tokenize(string.Empty);
            Assert.Equal(0, tokens.Count);
        }

        [Fact]
        public void TokenizeWord()
        {
            var lexer = new Lexer();
            IReadOnlyList<Token> tokens = lexer.Tokenize(" a b c ");
            Assert.Equal(7, tokens.Count);
            Assert.Equal(TokenType.Whitespace, tokens[0].Type);
            Assert.Equal(TokenType.Word, tokens[1].Type);
            Assert.Equal(TokenType.Whitespace, tokens[2].Type);
            Assert.Equal(TokenType.Word, tokens[3].Type);
            Assert.Equal(TokenType.Whitespace, tokens[4].Type);
            Assert.Equal(TokenType.Word, tokens[5].Type);
            Assert.Equal(TokenType.Whitespace, tokens[6].Type);

            tokens = lexer.Tokenize(" abæçØ ");
            Assert.Equal(3, tokens.Count);
            Assert.Equal(TokenType.Whitespace, tokens[0].Type);
            Assert.Equal(TokenType.Word, tokens[1].Type);
            Assert.Equal(1, tokens[1].StartIndex);
            Assert.Equal(5, tokens[1].Length);
            Assert.Equal(TokenType.Whitespace, tokens[2].Type);
            Assert.Equal(6, tokens[2].StartIndex);
        }

        [Fact]
        public void TokenizeNumber()
        {
            var lexer = new Lexer();
            IReadOnlyList<Token> tokens = lexer.Tokenize(" 1 2 3 ");
            Assert.Equal(7, tokens.Count);
            Assert.Equal(TokenType.Whitespace, tokens[0].Type);
            Assert.Equal(TokenType.Number, tokens[1].Type);
            Assert.Equal(TokenType.Whitespace, tokens[2].Type);
            Assert.Equal(TokenType.Number, tokens[3].Type);
            Assert.Equal(TokenType.Whitespace, tokens[4].Type);
            Assert.Equal(TokenType.Number, tokens[5].Type);
            Assert.Equal(TokenType.Whitespace, tokens[6].Type);

            tokens = lexer.Tokenize(" 12 ");
            Assert.Equal(3, tokens.Count);
            Assert.Equal(TokenType.Whitespace, tokens[0].Type);
            Assert.Equal(TokenType.Number, tokens[1].Type);
            Assert.Equal(1, tokens[1].StartIndex);
            Assert.Equal(2, tokens[1].Length);
            Assert.Equal(TokenType.Whitespace, tokens[2].Type);
            Assert.Equal(3, tokens[2].StartIndex);
        }

        [Fact]
        public void TokenizeWhitespace()
        {
            var lexer = new Lexer();
            IReadOnlyList<Token> tokens = lexer.Tokenize(" ");
            Assert.Equal(1, tokens.Count);
            Assert.Equal(TokenType.Whitespace, tokens[0].Type);
            Assert.Equal(0, tokens[0].StartIndex);
            Assert.Equal(1, tokens[0].Length);

            tokens = lexer.Tokenize("  ");
            Assert.Equal(1, tokens.Count);
            Assert.Equal(TokenType.Whitespace, tokens[0].Type);
            Assert.Equal(0, tokens[0].StartIndex);
            Assert.Equal(2, tokens[0].Length);

            tokens = lexer.Tokenize("   ");
            Assert.Equal(1, tokens.Count);
            Assert.Equal(TokenType.Whitespace, tokens[0].Type);
            Assert.Equal(0, tokens[0].StartIndex);
            Assert.Equal(3, tokens[0].Length);
        }

        [Fact]
        public void TokenizePunctuationAndSymbols()
        {
            var lexer = new Lexer();
            IReadOnlyList<Token> tokens = lexer.Tokenize("!@#$%^&*()+_=`~[]{}\\|;:'\",<.>/?");
            Assert.Equal(31, tokens.Count);
            for (int i = 0; i < tokens.Count; i++)
            {
                Assert.Equal(TokenType.SymbolOrPunctuation, tokens[i].Type);
                Assert.Equal(1, tokens[i].Length);
            }

            tokens = lexer.Tokenize("π÷–−×·⋅¿¾½¼»º¹¸·¶µ´³²±°¯®­¬«ª©¨§¦¥¤£¢¡~}|{");
            Assert.Equal(42, tokens.Count);
            for (int i = 0; i < tokens.Count; i++)
            {
                Assert.Equal(TokenType.SymbolOrPunctuation, tokens[i].Type);
                Assert.Equal(1, tokens[i].Length);
            }
        }
    }
}
