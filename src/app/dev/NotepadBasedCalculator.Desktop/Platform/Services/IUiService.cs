using Avalonia.Styling;
using NotepadBasedCalculator.Desktop.Platform.Services.Theme;

namespace NotepadBasedCalculator.Desktop.Platform.Services
{
    internal interface IUiService
    {
        /// <summary>
        /// Gets the list of UI styles to use on the platform, applying to the given <paramref name="theme"/>.
        /// </summary>
        IEnumerable<Styles> GetPlatformSpecificStyles(AppTheme theme);
    }
}
