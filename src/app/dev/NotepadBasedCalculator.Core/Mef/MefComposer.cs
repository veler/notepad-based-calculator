using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using NotepadBasedCalculator.BuiltInPlugins.Data;

namespace NotepadBasedCalculator.Core.Mef
{
    /// <summary>
    /// Provides a set of methods to initialize and manage MEF.
    /// </summary>
    internal sealed class MefComposer : IDisposable
    {
        private readonly Assembly[] _assemblies;
        private readonly object[] _customExports;
        private bool _isExportProviderDisposed = true;

        public IMefProvider Provider { get; }

        public ExportProvider ExportProvider { get; private set; }

        public MefComposer(Assembly[]? assemblies = null, params object[] customExports)
        {
            if (Provider is not null)
            {
                throw new InvalidOperationException("Mef composer already initialized.");
            }

            _assemblies = assemblies ?? Array.Empty<Assembly>();
            _customExports = customExports ?? Array.Empty<object>();
            ExportProvider = InitializeMef();

            Provider = ExportProvider.GetExport<IMefProvider>()!.Value;
            ((MefProvider)Provider).ExportProvider = ExportProvider;
        }

        public void Dispose()
        {
            if (ExportProvider is not null)
            {
                ((CompositionContainer)ExportProvider).Dispose();
            }

            _isExportProviderDisposed = true;
        }

        internal void Reset()
        {
            // For unit tests.
            Dispose();
            InitializeMef();
        }

        private ExportProvider InitializeMef()
        {
            if (!_isExportProviderDisposed)
            {
                return ExportProvider;
            }

            var assemblies = new HashSet<Assembly>(_assemblies)
            {
                Assembly.GetExecutingAssembly(),
                typeof(NumberDataParser).Assembly
            };

            var catalog = new AggregateCatalog();
            foreach (Assembly assembly in assemblies)
            {
                catalog.Catalogs.Add(new AssemblyCatalog(assembly));
            }

            var container = new CompositionContainer(catalog);
            var batch = new CompositionBatch();
            batch.AddPart(this);

            for (int i = 0; i < _customExports.Length; i++)
            {
                batch.AddPart(_customExports[i]);
            }

            container.Compose(batch);

            ExportProvider = container;

            _isExportProviderDisposed = false;

            return ExportProvider;
        }
    }
}
