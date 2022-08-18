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
