using Avalonia.Media;
using NotepadBasedCalculator.Desktop.Platform.Services.Theme;

namespace NotepadBasedCalculator.Desktop.Mac.Services
{
    [Export(typeof(IThemeService))]
    internal sealed class ThemeService : NSObject, IThemeService
    {
        private const string AppleInterfaceThemeChangedNotification = "AppleInterfaceThemeChangedNotification";
        private const string NSSystemColorsDidChangeNotification = "NSSystemColorsDidChangeNotification";

        public Themes AppTheme { get; set; }

        public AppThemes AppliedTheme { get; }

        public Color AccentColor { get; }

        public event EventHandler? ThemeChanged;

        public event EventHandler? AccentColorChanged;

        [ImportingConstructor]
        public ThemeService()
        {
            NSDistributedNotificationCenter.DefaultCenter.AddObserver(
                new NSString(AppleInterfaceThemeChangedNotification),
                OnThemeChanged);

            NSNotificationCenter.DefaultCenter.AddObserver(
                new NSString(NSSystemColorsDidChangeNotification),
                OnAccentColorChanged);
        }

        private void OnThemeChanged(NSNotification notification)
        {
            var interfaceStyle = NSUserDefaults.StandardUserDefaults.StringForKey("AppleInterfaceStyle");
        }

        private void OnAccentColorChanged(NSNotification notification)
        {
            NSColor.ControlAccent.ToString();
        }
    }
}
