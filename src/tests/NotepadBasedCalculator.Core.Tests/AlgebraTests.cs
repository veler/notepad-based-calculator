using System.Collections.Generic;
using System.Threading.Tasks;
using NotepadBasedCalculator.Api;
using Xunit;

namespace NotepadBasedCalculator.Core.Tests
{
    public sealed class AlgebraTests : MefBaseTest
    {
        private readonly Interpreter _interpreter;
        private readonly TextDocument _textDocument;

        public AlgebraTests()
        {
            InterpreterFactory interpreterFactory = ExportProvider.Import<InterpreterFactory>();
            _textDocument = new TextDocument();
            _interpreter = interpreterFactory.CreateInterpreter(SupportedCultures.English, _textDocument);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _interpreter.Dispose();
        }

        [Theory]
        [InlineData("1 + 25%", "1.25")]
        [InlineData("1.50 + 25%", "1.875")]
        [InlineData("1 + 1.25", "2.25")]
        [InlineData("1.50 + 1.25", "2.75")]
        [InlineData("1 + the half", "1.5")]
        [InlineData("a fifth + 2", "2.2")]
        [InlineData("1 + True", "2")]
        [InlineData("1 + False", "1")]
        [InlineData("1 - 25%", "0.75")]
        [InlineData("1.50 - 25%", "1.125")]
        [InlineData("1 - 1.25", "-0.25")]
        [InlineData("1-1.25", "-0.25")]
        [InlineData("1.50 - 1.25", "0.25")]
        [InlineData("1 - the half", "0.5")]
        [InlineData("1 - True", "0")]
        [InlineData("1 - False", "1")]
        [InlineData("1 x 25%", "0.25")]
        [InlineData("1.50 x 25%", "0.375")]
        [InlineData("1 x 1.25", "1.25")]
        [InlineData("1.50 x 1.25", "1.875")]
        [InlineData("1 x one third", "0.33333334")]
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

            IReadOnlyList<IData> lineResults = await _interpreter.WaitAsync();
            Assert.Single(lineResults);
            Assert.Equal(output, lineResults[0].DisplayText);
        }
    }
}
