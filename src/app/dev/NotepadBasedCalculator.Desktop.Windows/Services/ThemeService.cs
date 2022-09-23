using Avalonia.Media;
using NotepadBasedCalculator.Desktop.Platform.Services.Theme;

namespace NotepadBasedCalculator.Desktop.Windows.Services
{
    [Export(typeof(IThemeService))]
    internal sealed class ThemeService : IThemeService
    {
        public UserPreferredTheme UserPreferredTheme { get; set; }

        public AppTheme AppliedAppTheme { get; }

        public Color AccentColor { get; }

        public event EventHandler? ThemeChanged;

        public event EventHandler? AccentColorChanged;
    }
}
