using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NotepadBasedCalculator.Api;
using Xunit;

namespace NotepadBasedCalculator.Core.Tests
{
    public class FunctionTests : MefBaseTest
    {
        private readonly ParserAndInterpreter _parserAndInterpreter;
        private readonly TextDocument _textDocument;

        public FunctionTests()
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
        [InlineData("20% of 60", "12")]
        [InlineData("20% of 60km", "12 km")]
        [InlineData("20% of 25 + 50", "15")]
        [InlineData("tax = 40% \r\n tax of 75$", "30 Dollar")]
        [InlineData("20% of 20%", "0.04")]
        public async Task Percentage_PercentOf(string input, string output)
        {
            _textDocument.Text = input;
            IReadOnlyList<ParserAndInterpreterResultLine> lineResults = await _parserAndInterpreter.WaitAsync();
            Assert.Single(lineResults.Last().StatementsAndData);
            var statement = (FunctionStatement)lineResults.Last().StatementsAndData[0].ParsedStatement;
            Assert.Equal("percentage.percentOf", statement.FunctionDefinition.FunctionFullName);
            Assert.Equal(output, lineResults.Last().StatementsAndData[0].ResultedData.DisplayText);
        }

        [Theory]
        [InlineData("20% off 60", "48")]
        [InlineData("20% off 60km", "48 km")]
        [InlineData("20% off 25 + 50", "60")]
        [InlineData("tax = 40% \r\n tax off 75$", "45 Dollar")]
        [InlineData("20% off 20%", "0.16")]
        public async Task Percentage_PercentOff(string input, string output)
        {
            _textDocument.Text = input;
            IReadOnlyList<ParserAndInterpreterResultLine> lineResults = await _parserAndInterpreter.WaitAsync();
            Assert.Single(lineResults.Last().StatementsAndData);
            var statement = (FunctionStatement)lineResults.Last().StatementsAndData[0].ParsedStatement;
            Assert.Equal("percentage.percentOff", statement.FunctionDefinition.FunctionFullName);
            Assert.Equal(output, lineResults.Last().StatementsAndData[0].ResultedData.DisplayText);
        }

        [Theory]
        [InlineData("20% on 60", "72")]
        [InlineData("20% on 60km", "72 km")]
        [InlineData("20% on 25 + 50", "90")]
        [InlineData("tax = 40% \r\n tax on 75$", "105 Dollar")]
        [InlineData("20% on 20%", "0.24")]
        public async Task Percentage_PercentOn(string input, string output)
        {
            _textDocument.Text = input;
            IReadOnlyList<ParserAndInterpreterResultLine> lineResults = await _parserAndInterpreter.WaitAsync();
            Assert.Single(lineResults.Last().StatementsAndData);
            var statement = (FunctionStatement)lineResults.Last().StatementsAndData[0].ParsedStatement;
            Assert.Equal("percentage.percentOn", statement.FunctionDefinition.FunctionFullName);
            Assert.Equal(output, lineResults.Last().StatementsAndData[0].ResultedData.DisplayText);
        }

        [Theory]
        [InlineData("1km is what % of 10,000m", "10%")]
        [InlineData("1km is what percent of 10,000m", "10%")]
        [InlineData("1km is what percentage of 10,000m", "10%")]
        [InlineData("1km represents what % of 10,000m", "10%")]
        [InlineData("1km represents what percent of 10,000m", "10%")]
        [InlineData("1km represents what percentage of 10,000m", "10%")]
        [InlineData("1km off 10,000m is what %", "10%")]
        [InlineData("1km off 10,000m is what percent", "10%")]
        [InlineData("1km off 10,000m is what percentage", "10%")]
        [InlineData("1km as a % of 10,000m", "10%")]
        [InlineData("1km as % of 10,000m", "10%")]
        [InlineData("1km as a percent of 10,000m", "10%")]
        [InlineData("1km as percent of 10,000m", "10%")]
        [InlineData("1km as a percentage of 10,000m", "10%")]
        [InlineData("1km as percentage of 10,000m", "10%")]
        public async Task Percentage_IsWhatPercentOf(string input, string output)
        {
            _textDocument.Text = input;
            IReadOnlyList<ParserAndInterpreterResultLine> lineResults = await _parserAndInterpreter.WaitAsync();
            Assert.Single(lineResults.Last().StatementsAndData);
            var statement = (FunctionStatement)lineResults.Last().StatementsAndData[0].ParsedStatement;
            Assert.Equal("percentage.isWhatPercentOf", statement.FunctionDefinition.FunctionFullName);
            Assert.Equal(output, (double.Parse(lineResults.Last().StatementsAndData[0].ResultedData.DisplayText) * 100).ToString() + "%");
        }

        [Theory]
        [InlineData("1km is what % on 10,000m", "9%")]
        [InlineData("1km is what percent on 10,000m", "9%")]
        [InlineData("1km is what percentage on 10,000m", "9%")]
        [InlineData("1km as a % on 10,000m", "9%")]
        [InlineData("1km as % on 10,000m", "9%")]
        [InlineData("1km as a percent on 10,000m", "9%")]
        [InlineData("1km as percent on 10,000m", "9%")]
        [InlineData("1km as a percentage on 10,000m", "9%")]
        [InlineData("1km as percentage on 10,000m", "9%")]
        public async Task Percentage_IsWhatPercentOn(string input, string output)
        {
            _textDocument.Text = input;
            IReadOnlyList<ParserAndInterpreterResultLine> lineResults = await _parserAndInterpreter.WaitAsync();
            Assert.Single(lineResults.Last().StatementsAndData);
            var statement = (FunctionStatement)lineResults.Last().StatementsAndData[0].ParsedStatement;
            Assert.Equal("percentage.isWhatPercentOn", statement.FunctionDefinition.FunctionFullName);
            Assert.Equal(output, (double.Parse(lineResults.Last().StatementsAndData[0].ResultedData.DisplayText) * 100).ToString() + "%");
        }

        [Theory]
        [InlineData("1km is what % off 10,000m", "11%")]
        [InlineData("1km is what percent off 10,000m", "11%")]
        [InlineData("1km is what percentage off 10,000m", "11%")]
        [InlineData("1km as a % off 10,000m", "11%")]
        [InlineData("1km as % off 10,000m", "11%")]
        [InlineData("1km as a percent off 10,000m", "11%")]
        [InlineData("1km as percent off 10,000m", "11%")]
        [InlineData("1km as a percentage off 10,000m", "11%")]
        [InlineData("1km as percentage off 10,000m", "11%")]
        public async Task Percentage_IsWhatPercentOff(string input, string output)
        {
            _textDocument.Text = input;
            IReadOnlyList<ParserAndInterpreterResultLine> lineResults = await _parserAndInterpreter.WaitAsync();
            Assert.Single(lineResults.Last().StatementsAndData);
            var statement = (FunctionStatement)lineResults.Last().StatementsAndData[0].ParsedStatement;
            Assert.Equal("percentage.isWhatPercentOff", statement.FunctionDefinition.FunctionFullName);
            Assert.Equal(output, (double.Parse(lineResults.Last().StatementsAndData[0].ResultedData.DisplayText) * 100).ToString() + "%");
        }
    }
}
