using Avalonia.Media;
using NotepadBasedCalculator.Desktop.Platform.Services.Theme;

namespace NotepadBasedCalculator.Desktop.Mac.Services
{
    [Export(typeof(IThemeService))]
    internal sealed class ThemeService : IThemeService
    {
        private const string AppleInterfaceThemeChangedNotification = "AppleInterfaceThemeChangedNotification";
        private const string NSSystemColorsDidChangeNotification = "NSSystemColorsDidChangeNotification";
        private const string Dark = "Dark";
        private const string AppleInterfaceStyle = "AppleInterfaceStyle";

        public UserPreferredTheme UserPreferredTheme { get; set; }

        public AppTheme AppliedAppTheme { get; private set; }

        public Color AccentColor { get; private set; }

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
            
            UserPreferredTheme = UserPreferredTheme.Auto; // TODO: load from settings.
            AppliedAppTheme = DetectTheme();
			AccentColor = DetectAccentColor();
        }

        private void OnThemeChanged(NSNotification notification)
        {
            AppliedAppTheme = DetectTheme();
        }

        private void OnAccentColorChanged(NSNotification notification)
        {
			AccentColor = DetectAccentColor();
        }

        private AppTheme DetectTheme()
        {
            switch (UserPreferredTheme)
            {
                case UserPreferredTheme.Auto:
                    var style = NSUserDefaults.StandardUserDefaults.StringForKey("AppleInterfaceStyle");
                    if (style == Dark)
                    {
                        return AppTheme.Dark;
                    }
                    return AppTheme.Light;

                case UserPreferredTheme.Light:
                    return AppTheme.Light;

                case UserPreferredTheme.Dark:
                    return AppTheme.Dark;

                default:
                    ThrowHelper.ThrowNotSupportedException();
                    return AppTheme.Light;
            }
        }

        private Color DetectAccentColor()
        {
			var color = NSColor.ControlAccent.UsingColorSpace(NSColorSpace.GenericRGBColorSpace);
			color.GetRgba(out nfloat red, out nfloat green, out nfloat blue, out nfloat alpha);

			return new Color(
                (byte)(byte.MaxValue * alpha.Value),
                (byte)(byte.MaxValue * red.Value),
                (byte)(byte.MaxValue * green.Value),
                (byte)(byte.MaxValue * blue.Value));
        }
    }
}
