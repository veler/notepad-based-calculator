using System.Globalization;
using System.Text;
using Microsoft.Recognizers.Text;
using NotepadBasedCalculator.Api;
using NotepadBasedCalculator.Core;
using NotepadBasedCalculator.Core.Mef;
using Spectre.Console;

namespace NotepadBasedCalculator.StandaloneConsoleTestApp
{
    internal class Program
    {
        // Use English by default
        private const string DefaultCulture = Culture.English;

#pragma warning disable IDE0060 // Remove unused parameter
        public static void Main(string[] args)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync()
        {
            // Enable support for multiple encodings, especially in .NET Core
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var textDocument = new TextDocument();

            var mefComposer
                = new MefComposer(new[] { typeof(ConfigurationReader).Assembly });
            ParserAndInterpreterFactory parserAndInterpreterFactory = mefComposer.ExportProvider.GetExport<ParserAndInterpreterFactory>()!.Value;
            ParserAndInterpreter parserAndInterpreter = parserAndInterpreterFactory.CreateInstance(DefaultCulture, textDocument);

            Task warmupTask = WarmupAsync(textDocument, parserAndInterpreter);

            ShowIntro();

            while (true)
            {
                // Read the text to recognize
                string? input = AnsiConsole.Prompt(new TextPrompt<string?>("[bold yellow]>[/]").AllowEmpty());

                if (input?.ToLower(CultureInfo.InvariantCulture) == "exit")
                {
                    // Close application if user types "exit"
                    break;
                }

                try
                {
                    IReadOnlyList<ParserAndInterpreterResultLine>? results =
                        await AnsiConsole
                        .Status()
                        .AutoRefresh(true)
                        .Spinner(Spinner.Known.Dots2)
                        .StartAsync(
                            "Thinking...",
                            async ctx =>
                            {
                                await warmupTask;
                                textDocument.Text += input + Environment.NewLine;
                                return await parserAndInterpreter.WaitAsync();
                            });

                    if (results is not null && results.Count > 0 && results[0].SummarizedResultData is not null)
                    {
                        ParserAndInterpreterResultLine result = results[0];
                        bool isError = result.SummarizedResultData!.IsOfType(PredefinedTokenAndDataTypeNames.Error);
                        string output = result.SummarizedResultData.GetDisplayText(DefaultCulture);
                        if (!string.IsNullOrWhiteSpace(output))
                        {
                            AnsiConsole.Markup("[bold blue]=[/] ");
                            if (isError)
                            {
                                AnsiConsole.Markup($"[italic red1]{output}[/]");
                            }
                            else
                            {
                                AnsiConsole.Write(output);
                            }
                        }
                    }

                    AnsiConsole.WriteLine(string.Empty);
                }
                catch (Exception ex)
                {
                    AnsiConsole.WriteException(
                        ex,
                        ExceptionFormats.ShortenPaths
                        | ExceptionFormats.ShortenMethods
                        | ExceptionFormats.ShowLinks);
                }

                AnsiConsole.WriteLine();
            }
        }

        /// <summary>
        /// Introduction.
        /// </summary>
        private static void ShowIntro()
        {
            AnsiConsole.Write(new FigletText("Console-based calculator").LeftAligned());

            AnsiConsole.WriteLine("Enter a phrase and let us do the job for you.");
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine("Here are some examples you could try:");
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[grey62]\" What is 10 USD in EUR? \"[/]");
            AnsiConsole.MarkupLine("[grey62]\" Today + 3 months \"[/]");
            AnsiConsole.MarkupLine("[grey62]\" 10 km in m \"[/]");
            AnsiConsole.MarkupLine("[grey62]\" 3M + 10% \"[/]");
            AnsiConsole.MarkupLine("[grey62]\" 0.35 as % \"[/]");
            AnsiConsole.MarkupLine("[grey62]\" 20 tablespoons in teaspoons \"[/]");
            AnsiConsole.MarkupLine("[grey62]\" $30/day is what per year \"[/]");
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine("Enter something to calculate:");
        }

        private static async Task WarmupAsync(TextDocument textDocument, ParserAndInterpreter parserAndInterpreter)
        {
            textDocument.Text
                = @"average between 0 and 10
                    1000 m2 / 10 m2
                    June 23 2022 at 4pm
                    25 (50)
                    20h
                    01/01/2022
                    1km
                    1km/h
                    1kg
                    25%
                    123
                    1rad
                    2 km2
                    1 USD
                    a fifth
                    the third
                    1 MB
                    1F
                    if 20% off 60 + 50 equals 98 then tax = 12 else tax = 13
                    1 < True
                    if one hundred thousand dollars of income + (30% tax / two people) > 150k then test
                    7/1900";
            await parserAndInterpreter.WaitAsync();
            textDocument.Text = string.Empty;
        }
    }
}
