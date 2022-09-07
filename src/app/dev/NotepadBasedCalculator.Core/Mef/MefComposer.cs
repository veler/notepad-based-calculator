using System.Composition.Hosting;
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
        private bool _isExportProviderDisposed = true;

        public IMefProvider Provider { get; }

        public CompositionHost ExportProvider { get; private set; }

        public MefComposer(params Assembly[] assemblies)
        {
            if (Provider is not null)
            {
                throw new InvalidOperationException("Mef composer already initialized.");
            }

            _assemblies = assemblies;
            ExportProvider = InitializeMef();

            Provider = ExportProvider.GetExport<IMefProvider>();
            ((MefProvider)Provider).ExportProvider = ExportProvider;
        }

        public void Dispose()
        {
            if (ExportProvider is not null)
            {
                ExportProvider.Dispose();
            }

            _isExportProviderDisposed = true;
        }

        internal void Reset()
        {
            // For unit tests.
            Dispose();
            InitializeMef();
        }

        private CompositionHost InitializeMef()
        {
            if (!_isExportProviderDisposed)
            {
                return ExportProvider;
            }

            var assemblies = new HashSet<Assembly>(_assemblies);

            ContainerConfiguration? configuration
                = new ContainerConfiguration()
                    .WithAssembly(typeof(NumberDataParser).Assembly)
                    .WithAssemblies(assemblies);

            ExportProvider = configuration.CreateContainer();

            _isExportProviderDisposed = false;

            return ExportProvider;
        }
    }
}
