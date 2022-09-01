using System.Globalization;
using System.Text;
using Microsoft.Recognizers.Text;
using NotepadBasedCalculator.Core;
using NotepadBasedCalculator.Core.Mef;

namespace NotepadBasedCalculator.StandaloneConsoleTestApp
{
    internal class Program
    {
        // Use English by default
        private const string DefaultCulture = Culture.English;

        public static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync()
        {
            // Enable support for multiple encodings, especially in .NET Core
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var textDocument = new TextDocument();

            var mefComposer
                = new MefComposer(
                    typeof(MefComposer).Assembly);
            ParserAndInterpreterFactory parserAndInterpreterFactory = mefComposer.ExportProvider.GetExport<ParserAndInterpreterFactory>();
            ParserAndInterpreter parserAndInterpreter = parserAndInterpreterFactory.CreateInstance(DefaultCulture, textDocument);

            ShowIntro();

            while (true)
            {
                // Read the text to recognize
                Console.WriteLine("Enter something to calculate:");
                string? input = Console.ReadLine()?.Trim();

                if (input?.ToLower(CultureInfo.InvariantCulture) == "exit")
                {
                    // Close application if user types "exit"
                    break;
                }

                textDocument.Text += input + Environment.NewLine;

                IReadOnlyList<ParserAndInterpreterResultLine>? result = await parserAndInterpreter.WaitAsync();

                if (result is not null && result.Count > 0 && result[0].SummarizedResultData is not null)
                {
                    Console.WriteLine(">> " + result[0].SummarizedResultData!.GetDisplayText);
                }
                else
                {
                    Console.WriteLine(string.Empty);
                }

                Console.WriteLine();
            }
        }

        /// <summary>
        /// Introduction.
        /// </summary>
        private static void ShowIntro()
        {
            Console.WriteLine("Welcome to the Notepad-based calculator' Sample console application!");
            Console.WriteLine("To try the calculator enter a phrase and let us do the job for you.");
            Console.WriteLine();
            Console.WriteLine("Here are some examples you could try:");
            Console.WriteLine();
            Console.WriteLine("\" What is 10 USD in EUR? \"");
            Console.WriteLine("\" Today + 3 months \"");
            Console.WriteLine("\" 10 km in m \"");
            Console.WriteLine("\" 3M + 10% \"");
            Console.WriteLine("\" 0.35 as % \"");
            Console.WriteLine("\" 20 tablespoons in teaspoons \"");
            Console.WriteLine("\" $30/day is what per year \"");
            Console.WriteLine();
        }
    }
}
