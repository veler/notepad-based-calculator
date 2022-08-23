using System.Collections.Generic;
using System.Threading.Tasks;
using NotepadBasedCalculator.Api;
using Xunit;

namespace NotepadBasedCalculator.Core.Tests
{
    public sealed class InterpreterTests : MefBaseTest
    {
        [Fact]
        public async Task Intepreter_VariableDeclaration()
        {
            InterpreterFactory interpreterFactory = ExportProvider.Import<InterpreterFactory>();
            var textDocument = new TextDocument();
            using Interpreter interpreter = interpreterFactory.CreateInterpreter(SupportedCultures.English, textDocument);

            textDocument.Text = "test = 2";

            await interpreter.WaitAsync();
        }

        [Theory]
        [InlineData("30+30", "60")]
        [InlineData("30+20%", "36")]
        [InlineData("30 USD + 20%", "36 USD")]
        [InlineData("20%", "0.2")]
        [InlineData("20% + 20%", "0.4")]
        [InlineData("20% + 1", "1.2")]
        [InlineData("1 + 2 USD", "3 USD")]
        [InlineData("June 23 2022 at 4pm + 1h", "6/23/2022 5:00:00 PM")]
        [InlineData("(12)3+(1 +2)(3(2))(1 +2)-3", "87")]
        public async Task Intepreter_SimpleCalculus(string input, string output)
        {
            InterpreterFactory interpreterFactory = ExportProvider.Import<InterpreterFactory>();
            var textDocument = new TextDocument();
            using Interpreter interpreter = interpreterFactory.CreateInterpreter(SupportedCultures.English, textDocument);

            textDocument.Text = input;

            IReadOnlyList<IData> lineResults = await interpreter.WaitAsync();
            Assert.Single(lineResults);
            Assert.Equal(output, lineResults[0].DisplayText);
        }

        [Fact]
        public async Task Intepreter_DocumentChange()
        {
            InterpreterFactory interpreterFactory = ExportProvider.Import<InterpreterFactory>();
            var textDocument = new TextDocument();
            using Interpreter interpreter = interpreterFactory.CreateInterpreter(SupportedCultures.English, textDocument);

            TypeInDocument(textDocument, "test = 2");

            await interpreter.WaitAsync();
        }

        private static void TypeInDocument(TextDocument textDocument, string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                textDocument.Text += text[i];
            }
        }
    }
}
