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
            Assert.Equal(PredefinedTokenAndDataTypeNames.Whitespace, lines[0].Tokens[0].Type);
            Assert.Equal(PredefinedTokenAndDataTypeNames.Word, lines[0].Tokens[1].Type);
            Assert.Equal(PredefinedTokenAndDataTypeNames.Whitespace, lines[0].Tokens[2].Type);
            Assert.Equal(PredefinedTokenAndDataTypeNames.Word, lines[0].Tokens[3].Type);
            Assert.Equal(PredefinedTokenAndDataTypeNames.Whitespace, lines[0].Tokens[4].Type);
            Assert.Equal(PredefinedTokenAndDataTypeNames.Word, lines[0].Tokens[5].Type);
            Assert.Equal(PredefinedTokenAndDataTypeNames.Whitespace, lines[0].Tokens[6].Type);

            lines = Analyze("  abæçØ ");
            Assert.Equal(1, lines.Count);
            Assert.Equal(3, lines[0].Tokens.Count);
            Assert.Equal(PredefinedTokenAndDataTypeNames.Whitespace, lines[0].Tokens[0].Type);
            Assert.Equal(PredefinedTokenAndDataTypeNames.Word, lines[0].Tokens[1].Type);
            Assert.Equal(2, lines[0].Tokens[1].StartInLine);
            Assert.Equal(5, lines[0].Tokens[1].Length);
            Assert.Equal(PredefinedTokenAndDataTypeNames.Whitespace, lines[0].Tokens[2].Type);
            Assert.Equal(7, lines[0].Tokens[2].StartInLine);

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
            Assert.Equal(PredefinedTokenAndDataTypeNames.Whitespace, lines[0].Tokens[0].Type);
            Assert.Equal(PredefinedTokenAndDataTypeNames.Digit, lines[0].Tokens[1].Type);
            Assert.Equal(PredefinedTokenAndDataTypeNames.Whitespace, lines[0].Tokens[2].Type);
            Assert.Equal(PredefinedTokenAndDataTypeNames.Digit, lines[0].Tokens[3].Type);
            Assert.Equal(PredefinedTokenAndDataTypeNames.Whitespace, lines[0].Tokens[4].Type);
            Assert.Equal(PredefinedTokenAndDataTypeNames.Digit, lines[0].Tokens[5].Type);
            Assert.Equal(PredefinedTokenAndDataTypeNames.Whitespace, lines[0].Tokens[6].Type);

            lines = Analyze(" 12 ");
            Assert.Equal(1, lines.Count);
            Assert.Equal(4, lines[0].Tokens.Count);
            Assert.Equal(PredefinedTokenAndDataTypeNames.Whitespace, lines[0].Tokens[0].Type);
            Assert.Equal(PredefinedTokenAndDataTypeNames.Digit, lines[0].Tokens[1].Type);
            Assert.Equal(PredefinedTokenAndDataTypeNames.Digit, lines[0].Tokens[2].Type);
            Assert.Equal(1, lines[0].Tokens[1].StartInLine);
            Assert.Equal(1, lines[0].Tokens[1].Length);
            Assert.Equal(2, lines[0].Tokens[2].StartInLine);
            Assert.Equal(1, lines[0].Tokens[2].Length);
            Assert.Equal(PredefinedTokenAndDataTypeNames.Whitespace, lines[0].Tokens[3].Type);
            Assert.Equal(3, lines[0].Tokens[3].StartInLine);
        }

        [Fact]
        public void TokenizeWhitespace()
        {
            IReadOnlyList<LineInfo> lines = Analyze(" ");
            Assert.Equal(1, lines.Count);
            Assert.Equal(1, lines[0].Tokens.Count);
            Assert.Equal(PredefinedTokenAndDataTypeNames.Whitespace, lines[0].Tokens[0].Type);
            Assert.Equal(0, lines[0].Tokens[0].StartInLine);
            Assert.Equal(1, lines[0].Tokens[0].Length);

            lines = Analyze("  ");
            Assert.Equal(1, lines.Count);
            Assert.Equal(1, lines[0].Tokens.Count);
            Assert.Equal(PredefinedTokenAndDataTypeNames.Whitespace, lines[0].Tokens[0].Type);
            Assert.Equal(0, lines[0].Tokens[0].StartInLine);
            Assert.Equal(2, lines[0].Tokens[0].Length);

            lines = Analyze("   ");
            Assert.Equal(1, lines.Count);
            Assert.Equal(1, lines[0].Tokens.Count);
            Assert.Equal(PredefinedTokenAndDataTypeNames.Whitespace, lines[0].Tokens[0].Type);
            Assert.Equal(0, lines[0].Tokens[0].StartInLine);
            Assert.Equal(3, lines[0].Tokens[0].Length);

            lines = Analyze(" \t ");
            Assert.Equal(1, lines.Count);
            Assert.Equal(1, lines[0].Tokens.Count);
            Assert.Equal(PredefinedTokenAndDataTypeNames.Whitespace, lines[0].Tokens[0].Type);
            Assert.Equal(0, lines[0].Tokens[0].StartInLine);
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
                Assert.Equal(PredefinedTokenAndDataTypeNames.SymbolOrPunctuation, lines[0].Tokens[i].Type);
                Assert.Equal(1, lines[0].Tokens[i].Length);
            }

            lines = Analyze("π÷–−×·⋅¿¾½¼»º¹¸·¶µ´³²±°¯®­¬«ª©¨§¦¥¤£¢¡~}|{");
            Assert.Equal(1, lines.Count);
            Assert.Equal(42, lines[0].Tokens.Count);
            for (int i = 0; i < lines[0].Tokens.Count; i++)
            {
                Assert.Equal(PredefinedTokenAndDataTypeNames.SymbolOrPunctuation, lines[0].Tokens[i].Type);
                Assert.Equal(1, lines[0].Tokens[i].Length);
            }
        }

        [Fact]
        public void TokenizeMultipleLines()
        {
            IReadOnlyList<LineInfo> lines = Analyze("    \r\n\r\n\nabc\n  ");
            Assert.Equal(5, lines.Count);
            Assert.Equal(1, lines[0].TokenCount);
            Assert.Equal(PredefinedTokenAndDataTypeNames.Whitespace, lines[0].Tokens[0].Type);
            Assert.Equal(0, lines[1].TokenCount);
            Assert.Equal(0, lines[2].TokenCount);
            Assert.Equal(PredefinedTokenAndDataTypeNames.Word, lines[3].Tokens[0].Type);
            Assert.Equal(1, lines[4].TokenCount);
            Assert.Equal(PredefinedTokenAndDataTypeNames.Whitespace, lines[4].Tokens[0].Type);
        }

        private static IReadOnlyList<LineInfo> Analyze(string input)
        {
            var lines = new List<LineInfo>();
            IReadOnlyList<TokenizedTextLine> tokenizedLines = Lexer.Tokenize(input);

            for (int i = 0; i < tokenizedLines.Count; i++)
            {
                var tokens = new List<IToken>();

                LinkedToken linkedToken = tokenizedLines[i].Tokens;
                while (linkedToken is not null)
                {
                    tokens.Add(linkedToken.Token);
                    linkedToken = linkedToken.Next;
                }

                var lineInfo
                    = new LineInfo
                    {
                        LineNumber = i + 1,
                        Tokens = tokens,
                        TokenizedTextLine = tokenizedLines[i]
                    };
                lines.Add(lineInfo);
            }

            return lines;
        }

        private class LineInfo
        {
            internal TokenizedTextLine TokenizedTextLine { get; set; }

            internal IReadOnlyList<IToken> Tokens { get; set; }

            internal int TokenCount => Tokens.Count;

            internal int LineNumber { get; set; }
        }
    }
}
