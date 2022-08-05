using System.Composition.Hosting;
using System.Reflection;
using NotepadBasedCalculator.Api;
using NotepadBasedCalculator.BuiltInPlugins.Integer;

namespace NotepadBasedCalculator.Core.Mef
{
    /// <summary>
    /// Provides a set of methods to initialize and manage MEF.
    /// </summary>
    internal sealed class MefComposer : IDisposable
    {
        private readonly Assembly[] _assemblies;
        private bool isExportProviderDisposed = true;

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

            isExportProviderDisposed = true;
        }

        internal void Reset()
        {
            // For unit tests.
            Dispose();
            InitializeMef();
        }

        private CompositionHost InitializeMef()
        {
            if (!isExportProviderDisposed)
            {
                return ExportProvider;
            }

            var assemblies = new HashSet<Assembly>(_assemblies);

            ContainerConfiguration? configuration
                = new ContainerConfiguration()
                    .WithAssembly(typeof(IntegerExpressionParser).Assembly)
                    .WithAssemblies(assemblies);

            ExportProvider = configuration.CreateContainer();

            isExportProviderDisposed = false;

            return ExportProvider;
        }
    }
}
