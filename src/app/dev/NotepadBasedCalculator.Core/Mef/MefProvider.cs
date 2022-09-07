using System.Composition.Hosting;

namespace NotepadBasedCalculator.Core.Mef
{
    [Export(typeof(IMefProvider))]
    [Export(typeof(IServiceProvider))]
    [Shared]
    internal sealed class MefProvider : IMefProvider
    {
        internal CompositionHost? ExportProvider { get; set; }

        public object GetService(Type serviceType)
        {
            return ExportProvider!.GetExport(serviceType);
        }

        public TExport Import<TExport>()
        {
            return ExportProvider!.GetExport<TExport>();
        }

        public object Import(Type type)
        {
            return ExportProvider!.GetExport(type);
        }

        public IEnumerable<TExport> ImportMany<TExport>()
        {
            return ExportProvider!.GetExports<TExport>();
        }
    }
}
