﻿using System.Collections.Generic;
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
        [InlineData("250km is what % on 62,500m", "300%")]
        [InlineData("1,250m is what percent on 1km", "25%")]
        [InlineData("10,000m is what percentage on 1km", "900%")]
        [InlineData("10,000m as a % on 1km", "900%")]
        [InlineData("10,000m as % on 1km", "900%")]
        [InlineData("10,000m as a percent on 1km", "900%")]
        [InlineData("10,000m as percent on 1km", "900%")]
        [InlineData("10,000m as a percentage on 1km", "900%")]
        [InlineData("10,000m as percentage on 1km", "900%")]
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
        [InlineData("62.5km is what % off 250,000m", "75%")]
        [InlineData("1km is what percent off 10,000m", "90%")]
        [InlineData("1km is what percentage off 10,000m", "90%")]
        [InlineData("1km as a % off 10,000m", "90%")]
        [InlineData("1km as % off 10,000m", "90%")]
        [InlineData("1km as a percent off 10,000m", "90%")]
        [InlineData("1km as percent off 10,000m", "90%")]
        [InlineData("1km as a percentage off 10,000m", "90%")]
        [InlineData("1km as percentage off 10,000m", "90%")]
        public async Task Percentage_IsWhatPercentOff(string input, string output)
        {
            _textDocument.Text = input;
            IReadOnlyList<ParserAndInterpreterResultLine> lineResults = await _parserAndInterpreter.WaitAsync();
            Assert.Single(lineResults.Last().StatementsAndData);
            var statement = (FunctionStatement)lineResults.Last().StatementsAndData[0].ParsedStatement;
            Assert.Equal("percentage.isWhatPercentOff", statement.FunctionDefinition.FunctionFullName);
            Assert.Equal(output, (double.Parse(lineResults.Last().StatementsAndData[0].ResultedData.DisplayText) * 100).ToString() + "%");
        }

        [Theory]
        [InlineData("20 percent of what is 70", "350")]
        [InlineData("20 percent of what number is 70", "350")]
        [InlineData("70 is 20% of what", "350")]
        [InlineData("70 is 20% of what number", "350")]
        [InlineData("20 percent off what is 70", "350")]
        [InlineData("20 percent off what number is 70", "350")]
        [InlineData("70 is 20% off what", "350")]
        [InlineData("70 is 20% off what number", "350")]
        public async Task Percentage_IsPercentOfWhat(string input, string output)
        {
            _textDocument.Text = input;
            IReadOnlyList<ParserAndInterpreterResultLine> lineResults = await _parserAndInterpreter.WaitAsync();
            Assert.Single(lineResults.Last().StatementsAndData);
            var statement = (FunctionStatement)lineResults.Last().StatementsAndData[0].ParsedStatement;
            Assert.Equal("percentage.isPercentOfWhat", statement.FunctionDefinition.FunctionFullName);
            Assert.Equal(output, lineResults.Last().StatementsAndData[0].ResultedData.DisplayText);
        }

        [Theory]
        [InlineData("62.5 is 25% on what", "50")]
        [InlineData("62.5 is 25% on what number", "50")]
        [InlineData("62.5 is what plus 25%", "50")]
        [InlineData("25% on what is 62.5", "50")]
        [InlineData("25% on what number is 62.5", "50")]
        public async Task Percentage_IsPercentOnWhat(string input, string output)
        {
            _textDocument.Text = input;
            IReadOnlyList<ParserAndInterpreterResultLine> lineResults = await _parserAndInterpreter.WaitAsync();
            Assert.Single(lineResults.Last().StatementsAndData);
            var statement = (FunctionStatement)lineResults.Last().StatementsAndData[0].ParsedStatement;
            Assert.Equal("percentage.isPercentOnWhat", statement.FunctionDefinition.FunctionFullName);
            Assert.Equal(output, lineResults.Last().StatementsAndData[0].ResultedData.DisplayText);
        }

        [Fact]
        public async Task General_Random()
        {
            _textDocument.Text = "random number between 0 and 1";
            IReadOnlyList<ParserAndInterpreterResultLine> lineResults = await _parserAndInterpreter.WaitAsync();
            Assert.Single(lineResults.Last().StatementsAndData);
            var statement = (FunctionStatement)lineResults.Last().StatementsAndData[0].ParsedStatement;
            Assert.Equal("general.random", statement.FunctionDefinition.FunctionFullName);
            var number = (INumericData)lineResults.Last().StatementsAndData[0].ResultedData;
            Assert.InRange(number.NumericValue, 0d, 1d);

            _textDocument.Text = "random between 10km and 100000m";
            lineResults = await _parserAndInterpreter.WaitAsync();
            Assert.Single(lineResults.Last().StatementsAndData);
            statement = (FunctionStatement)lineResults.Last().StatementsAndData[0].ParsedStatement;
            Assert.Equal("general.random", statement.FunctionDefinition.FunctionFullName);
            number = (INumericData)lineResults.Last().StatementsAndData[0].ResultedData;
            Assert.InRange(number.NumericValue, 10, 100);
        }
    }
}