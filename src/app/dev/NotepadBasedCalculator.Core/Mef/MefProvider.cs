using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace NotepadBasedCalculator.Core.Mef
{
    [Export(typeof(IMefProvider))]
    internal sealed class MefProvider : IMefProvider
    {
        internal ExportProvider? ExportProvider { get; set; }

        public TExport Import<TExport>()
        {
            return ExportProvider!.GetExport<TExport>()!.Value;
        }

        public IEnumerable<TExport> ImportMany<TExport>()
        {
            return ExportProvider!.GetExports<TExport>().Select(e => e.Value);
        }
    }
}
