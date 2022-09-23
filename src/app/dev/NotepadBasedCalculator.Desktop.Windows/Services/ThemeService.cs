using Avalonia.Media;
using Microsoft.Win32;
using NotepadBasedCalculator.Desktop.Platform.Services.Theme;
using Windows.UI.ViewManagement;

namespace NotepadBasedCalculator.Desktop.Windows.Services
{
    [Export(typeof(IThemeService))]
    internal sealed class ThemeService : IThemeService
    {
        private const string SystemThemeRegistryKeyName = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
        private const string SystemThemeRegistryValueName = "SystemUsesLightTheme";

        private readonly object _syncObj = new();
        private readonly UISettings _uiSettings = new();

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
            _uiSettings.ColorValuesChanged += UiSettings_ColorValuesChanged;

            lock (_syncObj)
            {
                UserPreferredTheme = UserPreferredTheme.Auto; // TODO: load from settings.
                AppliedAppTheme = DetectTheme();
                AccentColor = DetectAccentColor();
            }
        }

        private void UiSettings_ColorValuesChanged(UISettings sender, object args)
        {
            lock (_syncObj)
            {
                AppliedAppTheme = DetectTheme();
                AccentColor = DetectAccentColor();
            }
        }

        private AppTheme DetectTheme()
        {
            switch (UserPreferredTheme)
            {
                case UserPreferredTheme.Auto:
                    if (Registry.GetValue(SystemThemeRegistryKeyName, SystemThemeRegistryValueName, null) is int theme)
                    {
                        if (theme == 0)
                        {
                            return AppTheme.Dark;
                        }
                        if (theme == 1)
                        {
                            return AppTheme.Light;
                        }
                    }
                    return AppliedAppTheme;

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
            global::Windows.UI.Color colorValue = _uiSettings.GetColorValue(UIColorType.Accent);
            return Color.FromArgb(colorValue.A, colorValue.R, colorValue.G, colorValue.B);
        }
    }
}
