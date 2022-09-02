using System.Threading.Tasks;
using NotepadBasedCalculator.Api;
using Xunit;

namespace NotepadBasedCalculator.Core.Tests
{
    public sealed class OperationHelperTests : MefBaseTest
    {
        private readonly ParserAndInterpreter _parserAndInterpreter;
        private readonly TextDocument _textDocument;

        public OperationHelperTests()
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
        [InlineData("1", BinaryOperatorType.Addition, "2", "3")]
        [InlineData("2km", BinaryOperatorType.Addition, "25%", "2.5 km")]
        [InlineData("25%", BinaryOperatorType.Addition, "2km", "2.5 km")]
        [InlineData("1km", BinaryOperatorType.Addition, "1 meter", "1.001 km")]
        [InlineData("1 meter", BinaryOperatorType.Addition, "1km", "1.001 km")]
        [InlineData("1km", BinaryOperatorType.Addition, "1 minute", "Incompatible units")]
        [InlineData("1km", BinaryOperatorType.Addition, "1", "2 km")]
        [InlineData("1", BinaryOperatorType.Addition, "1km", "2 km")]
        [InlineData("25%", BinaryOperatorType.Addition, "25%", "31.25%")]
        [InlineData("1", BinaryOperatorType.Subtraction, "2", "-1")]
        [InlineData("1km", BinaryOperatorType.Subtraction, "3km", "-2 km")]
        [InlineData("1", BinaryOperatorType.Subtraction, "3km", "-2 km")]
        [InlineData("3km", BinaryOperatorType.Subtraction, "25%", "2.25 km")]
        [InlineData("25%", BinaryOperatorType.Subtraction, "3km", "-3.75 km")]
        [InlineData("25%", BinaryOperatorType.Subtraction, "25%", "18.75%")]
        [InlineData("2", BinaryOperatorType.Multiply, "3", "6")]
        [InlineData("2km", BinaryOperatorType.Multiply, "3km", "6 km²")]
        [InlineData("1km", BinaryOperatorType.Multiply, "3km", "3 km\u00b2")]
        [InlineData("1kg", BinaryOperatorType.Multiply, "1kg", "Unsupported arithmetic operation")]
        [InlineData("2kg", BinaryOperatorType.Multiply, "3", "6 kg")]
        [InlineData("2km", BinaryOperatorType.Multiply, "3", "6 km")]
        [InlineData("2kg", BinaryOperatorType.Multiply, "25%", "0.5 kg")]
        [InlineData("2km", BinaryOperatorType.Multiply, "25%", "0.5 km")]
        [InlineData("25%", BinaryOperatorType.Multiply, "2km", "0.5 km")]
        [InlineData("25%", BinaryOperatorType.Multiply, "25%", "6.25%")]
        [InlineData("6", BinaryOperatorType.Division, "2", "3")]
        [InlineData("1km", BinaryOperatorType.Division, "0", "∞")]
        [InlineData("1km", BinaryOperatorType.Division, "3", "0.3333 km")]
        [InlineData("1km", BinaryOperatorType.Division, "3 meter", "333.3333333333")]
        [InlineData("1km", BinaryOperatorType.Division, "3km", "0.3333333333")]
        [InlineData("25%", BinaryOperatorType.Division, "25%", "100%")]
        [InlineData("1km", BinaryOperatorType.LessThan, "3km", "True")]
        [InlineData("1km", BinaryOperatorType.Equality, "3km", "False")]
        [InlineData("1km", BinaryOperatorType.LessThan, "3kg", "Incompatible units")]
        [InlineData("10 rad", BinaryOperatorType.Division, "20 rad", "0.5")]
        [InlineData("10 rad", BinaryOperatorType.Multiply, "20 rad", "Unsupported arithmetic operation")]
        [InlineData("10 rad", BinaryOperatorType.Addition, "20 rad", "30 rad")]
        [InlineData("10 rad", BinaryOperatorType.Subtraction, "20 rad", "-10 rad")]
        [InlineData("10 km2", BinaryOperatorType.Division, "20", "0.5 km²")]
        [InlineData("10 km2", BinaryOperatorType.Division, "20 km2", "0.5")]
        [InlineData("10 km2", BinaryOperatorType.Multiply, "20 km2", "Unsupported arithmetic operation")]
        [InlineData("10 km2", BinaryOperatorType.Multiply, "20", "200 km²")]
        [InlineData("10 km2", BinaryOperatorType.Addition, "20 km2", "30 km²")]
        [InlineData("10 km2", BinaryOperatorType.Subtraction, "20 km2", "-10 km²")]
        [InlineData("10F", BinaryOperatorType.Division, "20F", "Unsupported arithmetic operation")]
        [InlineData("10F", BinaryOperatorType.Multiply, "20F", "Unsupported arithmetic operation")]
        [InlineData("10F", BinaryOperatorType.Addition, "20F", "30 °F")]
        [InlineData("10F", BinaryOperatorType.Addition, "20", "30 °F")]
        [InlineData("32F", BinaryOperatorType.Addition, "20C", "20 °C")] // This may be better than what Soulver and Wolfram Alpha is doing
        [InlineData("10F", BinaryOperatorType.Subtraction, "20F", "-10 °F")]
        public async Task Operation(string inputLeft, BinaryOperatorType binaryOperatorType, string inputRight, string output)
        {
            _textDocument.Text = inputLeft;
            System.Collections.Generic.IReadOnlyList<ParserAndInterpreterResultLine> parsingResult = await _parserAndInterpreter.WaitAsync();
            var leftData = (IData)parsingResult[0].TokenizedTextLine.Tokens.Token;

            _textDocument.Text = inputRight.PadLeft(inputLeft.Length + 1 + inputRight.Length /* for operator */);
            parsingResult = await _parserAndInterpreter.WaitAsync();
            var rightData = (IData)parsingResult[0].TokenizedTextLine.Tokens.Token;

            try
            {
                IData result = OperationHelper.PerformOperation(leftData, binaryOperatorType, rightData);
                Assert.Equal(0, result.StartInLine);
                Assert.Equal(inputLeft.Length + 1 + inputRight.Length, result.Length);
                Assert.Equal(output, result.GetDataDisplayText());
            }
            catch (DataOperationException ex)
            {
                Assert.Equal(output, ex.GetLocalizedMessage(SupportedCultures.English));
            }
        }
    }
}
