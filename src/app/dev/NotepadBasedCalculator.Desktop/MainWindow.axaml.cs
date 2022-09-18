using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Threading;
using NotepadBasedCalculator.Core;
using NotepadBasedCalculator.Core.Mef;

namespace NotepadBasedCalculator.Desktop
{
    public partial class MainWindow : Window
    {
        private readonly TextDocument _textDocument = new();
        private readonly ParserAndInterpreter _parserAndInterpreter;

        public ObservableCollection<string> ResultList { get; } = new();

        public MainWindow()
        {
            InitializeComponent();

            var mefComposer = new MefComposer();
            ParserAndInterpreterFactory parserAndInterpreterFactory = mefComposer.ExportProvider.GetExport<ParserAndInterpreterFactory>()!.Value;
            _parserAndInterpreter = parserAndInterpreterFactory.CreateInstance(SupportedCultures.English, _textDocument);
            Editor.Document.TextChanged += Document_TextChanged;
            _parserAndInterpreter.ParserAndInterpreterResultUpdated += ParserAndInterpreter_ParserAndInterpreterResultUpdated;
        }

        private void Document_TextChanged(object? sender, EventArgs e)
        {
            _textDocument.Text = Editor.Document.Text;
        }

        private void ParserAndInterpreter_ParserAndInterpreterResultUpdated(object? sender, ParserAndInterpreterResultUpdatedEventArgs e)
        {
            Dispatcher.UIThread.Post(() =>
            {
                if (!e.Cancel)
                {
                    ResultList.Clear();
                    for (int i = 0; i < e.ResultPerLines?.Count; i++)
                    {
                        ParserAndInterpreterResultLine item = e.ResultPerLines[i];
                        if (item.SummarizedResultData is not null)
                        {
                            ResultList.Add(item.SummarizedResultData.GetDisplayText(SupportedCultures.English));
                        }
                        else
                        {
                            ResultList.Add("    ");
                        }
                    }
                }
            });
        }
    }
}
