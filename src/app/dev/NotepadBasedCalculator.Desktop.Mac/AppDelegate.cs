namespace NotepadBasedCalculator.Desktop.Mac
{
    [Register("AppDelegate")]
    public class AppDelegate : NSApplicationDelegate
    {
        private static bool isInitialized;

        internal static AppDelegate? Instance { get; private set; }

        internal static AppDelegate Init()
        {
            if (!isInitialized)
            {
                isInitialized = true;
                NSApplication.Init();
                Instance = new();
                NSApplication.SharedApplication.Delegate = Instance;
            }

            Guard.IsNotNull(Instance);
            return Instance!;
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            // Insert code here to initialize your application
        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }
    }
}
