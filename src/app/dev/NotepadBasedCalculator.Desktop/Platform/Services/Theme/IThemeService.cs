using Avalonia.Media;

namespace NotepadBasedCalculator.Desktop.Platform.Services.Theme
{
    internal interface IThemeService
    {
        event EventHandler ThemeChanged;

        event EventHandler AccentColorChanged;

        UserPreferredTheme UserPreferredTheme { get; set; }

        AppTheme AppliedAppTheme { get; }

        Color AccentColor { get; }
    }
}
