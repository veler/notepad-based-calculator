using System.Collections.Generic;
using NotepadBasedCalculator.Api;
using Xunit;

namespace NotepadBasedCalculator.Core.Tests
{
    public class LexerTests : MefBaseTest
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
            Assert.Equal(3, lines[0].TokenCount);
            Assert.True(lines[0].Tokens[0].Is(PredefinedTokenAndDataTypeNames.Word));
            Assert.True(lines[0].Tokens[1].Is(PredefinedTokenAndDataTypeNames.Word));
            Assert.True(lines[0].Tokens[2].Is(PredefinedTokenAndDataTypeNames.Word));

            lines = Analyze("  abæçØ ");
            Assert.Equal(1, lines.Count);
            Assert.Equal(1, lines[0].Tokens.Count);
            Assert.True(lines[0].Tokens[0].Is(PredefinedTokenAndDataTypeNames.Word));
            Assert.Equal(2, lines[0].Tokens[0].StartInLine);
            Assert.Equal(5, lines[0].Tokens[0].Length);

            Assert.True(lines[0].Tokens[0].IsTokenTextEqualTo("abæçØ", System.StringComparison.Ordinal));
            Assert.False(lines[0].Tokens[0].IsTokenTextEqualTo("a", System.StringComparison.Ordinal));
            Assert.False(lines[0].Tokens[0].IsTokenTextEqualTo("bæçØ", System.StringComparison.Ordinal));
            Assert.False(lines[0].Tokens[0].IsTokenTextEqualTo("baæçØ", System.StringComparison.Ordinal));
        }

        [Fact]
        public void TokenizeNumber()
        {
            IReadOnlyList<LineInfo> lines = Analyze(" 1 2 3 ");
            Assert.Equal(1, lines.Count);
            Assert.Equal(3, lines[0].Tokens.Count);
            Assert.True(lines[0].Tokens[0].Is(PredefinedTokenAndDataTypeNames.Digit));
            Assert.True(lines[0].Tokens[1].Is(PredefinedTokenAndDataTypeNames.Digit));
            Assert.True(lines[0].Tokens[2].Is(PredefinedTokenAndDataTypeNames.Digit));

            lines = Analyze(" 12 ");
            Assert.Equal(1, lines.Count);
            Assert.Equal(1, lines[0].Tokens.Count);
            Assert.True(lines[0].Tokens[0].Is(PredefinedTokenAndDataTypeNames.Digit));
            Assert.Equal(1, lines[0].Tokens[0].StartInLine);
            Assert.Equal(2, lines[0].Tokens[0].Length);
            Assert.Equal(2, lines[0].Tokens[0].Length);
        }

        [Fact]
        public void TokenizeWhitespace()
        {
            IReadOnlyList<LineInfo> lines = Analyze(" ");
            Assert.Equal(1, lines.Count);
            Assert.Equal(0, lines[0].Tokens.Count);

            lines = Analyze("  ");
            Assert.Equal(1, lines.Count);
            Assert.Equal(0, lines[0].Tokens.Count);

            lines = Analyze("   ");
            Assert.Equal(1, lines.Count);
            Assert.Equal(0, lines[0].Tokens.Count);

            lines = Analyze(" \t ");
            Assert.Equal(1, lines.Count);
            Assert.Equal(0, lines[0].Tokens.Count);
        }

        [Fact]
        public void TokenizePunctuationAndSymbols()
        {
            IReadOnlyList<LineInfo> lines = Analyze("!@#$%^&*()+_=`~[]{}\\|;:'\",<.>/?");
            Assert.Equal(1, lines.Count);
            Assert.Equal(1, lines[0].Tokens.Count);
            Assert.Equal("[SymbolOrPunctuation] (0, 31): '!@#$%^&*()+_=`~[]{}\\|;:'\",<.>/?'", lines[0].Tokens[0].ToString());

            lines = Analyze("π÷–−×·⋅¿¾½¼»º¹¸·¶µ´³²±°¯®­¬«ª©¨§¦¥¤£¢¡~}|{");
            Assert.Equal(1, lines.Count);
            Assert.Equal(1, lines[0].Tokens.Count);
            Assert.Equal("[SymbolOrPunctuation] (0, 42): 'π÷–−×·⋅¿¾½¼»º¹¸·¶µ´³²±°¯®­¬«ª©¨§¦¥¤£¢¡~}|{'", lines[0].Tokens[0].ToString());
        }

        [Fact]
        public void TokenizeMultipleLines()
        {
            IReadOnlyList<LineInfo> lines = Analyze("    \r\n\r\n\nabc\n  ");
            Assert.Equal(5, lines.Count);
            Assert.Equal(0, lines[0].TokenCount);
            Assert.Equal(0, lines[1].TokenCount);
            Assert.Equal(0, lines[2].TokenCount);
            Assert.True(lines[3].Tokens[0].Is(PredefinedTokenAndDataTypeNames.Word));
            Assert.Equal(0, lines[4].TokenCount);
        }

        private IReadOnlyList<LineInfo> Analyze(string input)
        {
            var lines = new List<LineInfo>();
            IReadOnlyList<TokenizedTextLine> tokenizedLines = ExportProvider.Import<Lexer>().Tokenize(SupportedCultures.English, input);

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
