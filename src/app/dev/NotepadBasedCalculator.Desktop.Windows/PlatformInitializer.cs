using NotepadBasedCalculator.Desktop.Platform;

namespace NotepadBasedCalculator.Desktop.Windows
{
    [Export(typeof(IPlatformInitializer))]
    internal sealed class PlatformInitializer : IPlatformInitializer
    {
        private bool _isInitialized;

        public void Initialize()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
            }
        }
    }
}
