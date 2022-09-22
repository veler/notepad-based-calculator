using Avalonia.Media;
using NotepadBasedCalculator.Desktop.Platform.Services.Theme;

namespace NotepadBasedCalculator.Desktop.Windows.Services
{
    [Export(typeof(IThemeService))]
    internal sealed class ThemeService : IThemeService
    {
        public Themes AppTheme { get; set; }

        public AppThemes AppliedTheme { get; }

        public Color AccentColor { get; }

        public event EventHandler? ThemeChanged;

        public event EventHandler? AccentColorChanged;
    }
}
