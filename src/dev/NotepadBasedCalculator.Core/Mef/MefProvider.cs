using System.Composition.Hosting;
using NotepadBasedCalculator.Api;

namespace NotepadBasedCalculator.Core.Mef
{
    [Export(typeof(IMefProvider))]
    [Shared]
    internal sealed class MefProvider : IMefProvider
    {
        internal CompositionHost? ExportProvider { get; set; }

        public TExport Import<TExport>()
        {
            return ExportProvider!.GetExport<TExport>();
        }

        public IEnumerable<TExport> ImportMany<TExport>()
        {
            return ExportProvider!.GetExports<TExport>();
        }
    }
}
