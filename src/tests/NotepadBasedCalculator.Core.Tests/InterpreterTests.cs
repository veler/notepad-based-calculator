using System.Threading.Tasks;
using NotepadBasedCalculator.Api;
using Xunit;

namespace NotepadBasedCalculator.Core.Tests
{
    public sealed class InterpreterTests : MefBaseTest
    {
        [Fact]
        public async Task Intepreter_DocumentChange()
        {
            InterpreterFactory interpreterFactory = ExportProvider.Import<InterpreterFactory>();
            var textDocument = new TextDocument();
            Interpreter interpreter = interpreterFactory.CreateInterpreter(SupportedCultures.English, textDocument);

            AppendToTextDocument(textDocument, "123");

            await Task.Delay(5000);
        }

        private void AppendToTextDocument(TextDocument textDocument, string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                textDocument.Text += text[i];
            }
        }
    }
}
