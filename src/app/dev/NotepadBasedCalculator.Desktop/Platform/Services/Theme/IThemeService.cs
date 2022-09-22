using Avalonia.Media;

namespace NotepadBasedCalculator.Desktop.Platform.Services.Theme
{
    internal interface IThemeService
    {
        event EventHandler ThemeChanged;

        event EventHandler AccentColorChanged;

        Themes AppTheme { get; set; }

        AppThemes AppliedTheme { get; }

        Color AccentColor { get; }
    }
}
