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

        private readonly object _syncObj = new();

        private UserPreferredTheme _userPreferredTheme;
        private AppTheme _appliedAppTheme;
        private Color _accentColor;

        public UserPreferredTheme UserPreferredTheme
        {
            get => _userPreferredTheme;
            set
            {
                if (_userPreferredTheme != value)
                {
                    _userPreferredTheme = value;
                    lock (_syncObj)
                    {
                        AppliedAppTheme = DetectTheme();
                        AccentColor = DetectAccentColor();
                    }
                    // TODO: save setting.
                }
            }
        }

        public AppTheme AppliedAppTheme
        {
            get => _appliedAppTheme;
            private set
            {
                if (_appliedAppTheme != value)
                {
                    _appliedAppTheme = value;
                    ThemeChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public Color AccentColor
        {
            get => _accentColor;
            private set
            {
                if (_accentColor != value)
                {
                    _accentColor = value;
                    AccentColorChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

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

            lock (_syncObj)
            {
                UserPreferredTheme = UserPreferredTheme.Auto; // TODO: load from settings.
                AppliedAppTheme = DetectTheme();
                AccentColor = DetectAccentColor();
            }
        }

        private void OnThemeChanged(NSNotification notification)
        {
            lock (_syncObj)
            {
                AppliedAppTheme = DetectTheme();
            }
        }

        private void OnAccentColorChanged(NSNotification notification)
        {
            lock (_syncObj)
            {
                AccentColor = DetectAccentColor();
            }
        }

        private AppTheme DetectTheme()
        {
            switch (UserPreferredTheme)
            {
                case UserPreferredTheme.Auto:
                    string style = NSUserDefaults.StandardUserDefaults.StringForKey(AppleInterfaceStyle);
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

        private static Color DetectAccentColor()
        {
            NSColor color = NSColor.ControlAccent.UsingColorSpace(NSColorSpace.GenericRGBColorSpace);
            color.GetRgba(out nfloat red, out nfloat green, out nfloat blue, out nfloat alpha);

            return new Color(
                (byte)(byte.MaxValue * alpha.Value),
                (byte)(byte.MaxValue * red.Value),
                (byte)(byte.MaxValue * green.Value),
                (byte)(byte.MaxValue * blue.Value));
        }
    }
}
