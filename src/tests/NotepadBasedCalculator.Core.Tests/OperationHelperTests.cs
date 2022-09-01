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
        [InlineData("1km", BinaryOperatorType.Addition, "1 minute", "Error: incompatible units")]
        [InlineData("1km", BinaryOperatorType.Addition, "1", "2 km")]
        [InlineData("1", BinaryOperatorType.Addition, "1km", "2 km")]
        [InlineData("25%", BinaryOperatorType.Addition, "25%", "31.25%")]
        [InlineData("1km", BinaryOperatorType.Subtraction, "3km", "-2 km")]
        [InlineData("1", BinaryOperatorType.Subtraction, "3km", "-2 km")]
        [InlineData("3km", BinaryOperatorType.Subtraction, "25%", "2.25 km")]
        [InlineData("25%", BinaryOperatorType.Subtraction, "3km", "-3.75 km")]
        [InlineData("25%", BinaryOperatorType.Subtraction, "25%", "18.75%")]
        [InlineData("2km", BinaryOperatorType.Multiply, "3km", "6 km²")]
        [InlineData("1km", BinaryOperatorType.Multiply, "3km", "3 km\u00b2")]
        [InlineData("1kg", BinaryOperatorType.Multiply, "1kg", "Error: incompatible units")]
        [InlineData("2kg", BinaryOperatorType.Multiply, "3", "6 kg")]
        [InlineData("2km", BinaryOperatorType.Multiply, "3", "6 km")]
        [InlineData("2kg", BinaryOperatorType.Multiply, "25%", "0.5 kg")]
        [InlineData("2km", BinaryOperatorType.Multiply, "25%", "0.5 km")]
        [InlineData("25%", BinaryOperatorType.Multiply, "2km", "0.5 km")]
        [InlineData("25%", BinaryOperatorType.Multiply, "25%", "6.25%")]
        [InlineData("1km", BinaryOperatorType.Division, "0", "∞")]
        [InlineData("1km", BinaryOperatorType.Division, "3", "0.3333333333")]
        [InlineData("1km", BinaryOperatorType.Division, "3 meter", "333.3333333333")]
        [InlineData("1km", BinaryOperatorType.Division, "3km", "0.3333333333")]
        [InlineData("25%", BinaryOperatorType.Division, "25%", "1")]
        [InlineData("1km", BinaryOperatorType.LessThan, "3km", "True")]
        [InlineData("1km", BinaryOperatorType.Equality, "3km", "False")]
        [InlineData("1km", BinaryOperatorType.LessThan, "3kg", "Error: incompatible units")]
        public async Task Operation(string inputLeft, BinaryOperatorType binaryOperatorType, string inputRight, string output)
        {
            _textDocument.Text = inputLeft;
            System.Collections.Generic.IReadOnlyList<ParserAndInterpreterResultLine> parsingResult = await _parserAndInterpreter.WaitAsync();
            var leftData = (IData)parsingResult[0].TokenizedTextLine.Tokens.Token;

            _textDocument.Text = inputRight.PadLeft(inputLeft.Length + 1 + inputRight.Length /* for operator */);
            parsingResult = await _parserAndInterpreter.WaitAsync();
            var rightData = (IData)parsingResult[0].TokenizedTextLine.Tokens.Token;

            IData result = OperationHelper.NewPerformOperation(leftData, binaryOperatorType, rightData);
            Assert.Equal(output, result.GetDataDisplayText());
        }
    }
}
