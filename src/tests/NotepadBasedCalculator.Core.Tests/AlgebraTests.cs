using System.Collections.Generic;
using System.Threading.Tasks;
using NotepadBasedCalculator.Api;
using Xunit;

namespace NotepadBasedCalculator.Core.Tests
{
    public sealed class AlgebraTests : MefBaseTest
    {
        private readonly ParserAndInterpreter _parserAndInterpreter;
        private readonly TextDocument _textDocument;

        public AlgebraTests()
        {
            ParserAndInterpreterFactory parserAndInterpreterFactory = ExportProvider.Import<ParserAndInterpreterFactory>();
            _textDocument = new TextDocument();
            _parserAndInterpreter = parserAndInterpreterFactory.CreateInstance(SupportedCultures.English, _textDocument);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _parserAndInterpreter.Dispose();
        }

        [Theory]
        [InlineData("1 + 25%", "1.25")]
        [InlineData("1.50 + 25%", "1.875")]
        [InlineData("1 + 1.25", "2.25")]
        [InlineData("1.50 + 1.25", "2.75")]
        [InlineData("2 + the half", "3")]
        [InlineData("2km + the half", "3 km")]
        [InlineData("a fifth + 2", "2.2")]
        [InlineData("1 + True", "2")]
        [InlineData("1 + False", "1")]
        [InlineData("1 - 25%", "0.75")]
        [InlineData("1.50 - 25%", "1.125")]
        [InlineData("1 - 1.25", "-0.25")]
        [InlineData("1-1.25", "-0.25")]
        [InlineData("1.50 - 1.25", "0.25")]
        [InlineData("2 - the half", "1")]
        [InlineData("2km - the half", "1 km")]
        [InlineData("1 - True", "0")]
        [InlineData("1 - False", "1")]
        [InlineData("1 x 25%", "0.25")]
        [InlineData("1.50 x 25%", "0.375")]
        [InlineData("1 x 1.25", "1.25")]
        [InlineData("1.50 x 1.25", "1.875")]
        [InlineData("1 x one third", "0.3333333333")]
        [InlineData("1 x True", "1")]
        [InlineData("1 x False", "0")]
        [InlineData("True + True", "2")]
        [InlineData("True + False", "True")]
        [InlineData("False + False", "False")]
        [InlineData("True + 1", "2")]
        [InlineData("True + 0", "True")]
        [InlineData("1 / 25%", "4")]
        [InlineData("1.50 / 25%", "6")]
        [InlineData("1 / 1.25", "0.8")]
        [InlineData("1.50 / 1.25", "1.2")]
        [InlineData("1.50 / 0", "∞")]
        [InlineData("True / True", "True")]
        [InlineData("True / False", "∞")]
        public async Task AlgebraAsync(string input, string output)
        {
            _textDocument.Text = input;
            IReadOnlyList<ParserAndInterpreterResultLine> lineResults = await _parserAndInterpreter.WaitAsync();
            Assert.Single(lineResults);
            Assert.Single(lineResults[0].StatementsAndData);
            Assert.Equal(output, lineResults[0].StatementsAndData[0].ResultedData.GetDataDisplayText());
        }
    }
}
