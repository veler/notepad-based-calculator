using System.ComponentModel.Composition;
using NotepadBasedCalculator.Desktop.Platform;

namespace NotepadBasedCalculator.Desktop.Mac
{
    [Export(typeof(IPlatformInitializer))]
    internal sealed class PlatformInitializer : IPlatformInitializer
    {
        private bool isInitialized;

        public void Initialize()
        {
            if (!isInitialized)
            {
                isInitialized = true;
                AppDelegate.Init();
            }
        }
    }
}
