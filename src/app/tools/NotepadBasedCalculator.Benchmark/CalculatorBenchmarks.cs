using System.Text;
using BenchmarkDotNet.Attributes;
using Microsoft.Recognizers.Text;
using NotepadBasedCalculator.Core;
using NotepadBasedCalculator.Core.Mef;

namespace NotepadBasedCalculator.Benchmark
{
    public class CalculatorBenchmarks
    {
        // Use English by default
        private const string DefaultCulture = Culture.English;

        private TextDocument _textDocument;
        private ParserAndInterpreterFactory _parserAndInterpreterFactory;
        private ParserAndInterpreter _parserAndInterpreter;

        [Params(100)]
        public int IterationCount { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            // Enable support for multiple encodings, especially in .NET Core
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var mefComposer
                = new MefComposer();
            _parserAndInterpreterFactory = mefComposer.ExportProvider.GetExport<ParserAndInterpreterFactory>()!.Value;
        }

        [IterationSetup]
        public void IterationSetup() => WarmupAsync().Wait();

        [IterationCleanup]
        public void IterationCleanup()
        {
            _parserAndInterpreter.Dispose();
            _parserAndInterpreter = null;
        }

        [Benchmark(Description = "Calculator.Arithmetic")]
        public async Task CalculatorArithmeticAsync()
        {
            for (int i = 0; i < IterationCount; i++)
            {
                _textDocument.Text += "(12)3+(1 +2)(3(2))(1 +2)-3" + Environment.NewLine;
                await _parserAndInterpreter.WaitAsync();
            }
        }

        [Benchmark(Description = "Calculator.ArithmeticTypeSuperFast")]
        public async Task CalculatorArithmeticTypeSuperFastAsync()
        {
            for (int i = 0; i < IterationCount; i++)
            {
                _textDocument.Text += "(12)3+(1 +2)(3(2))(1 +2)-3" + Environment.NewLine;
            }

            await _parserAndInterpreter.WaitAsync();
        }

        private async Task WarmupAsync()
        {
            _textDocument = new TextDocument();
            _parserAndInterpreter = _parserAndInterpreterFactory.CreateInstance(DefaultCulture, _textDocument);
            _textDocument.Text = "20h; 01/01/2022; 1km; 1km/h; 1kg; 25%; 123; 1rad; 2 km2; 1 USD; a fifth; the third; 1 MB; 1F";
            await _parserAndInterpreter.WaitAsync();
            _textDocument.Text = string.Empty;
        }
    }
}
