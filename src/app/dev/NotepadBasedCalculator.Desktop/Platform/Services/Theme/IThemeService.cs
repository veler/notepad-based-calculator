using Avalonia.Media;

namespace NotepadBasedCalculator.Desktop.Platform.Services.Theme
{
    /// <summary>
    /// Represents a service for managing the theme of the application.
    /// </summary>
    internal interface IThemeService
    {
        /// <summary>
        /// Gets or sets the theme defined by the user.
        /// </summary>
        UserPreferredTheme UserPreferredTheme { get; set; }

        /// <summary>
        /// Gets the theme to use in the app.
        /// </summary>
        AppTheme AppliedAppTheme { get; }

        /// <summary>
        /// Gets the system's accent color.
        /// </summary>
        Color AccentColor { get; }

        /// <summary>
        /// Raised when <see cref="AppliedAppTheme"/> has changed.
        /// </summary>
        event EventHandler AppliedAppThemeChanged;

        /// <summary>
        /// Raised when <see cref="AccentColor"/> has changed.
        /// </summary>
        event EventHandler AccentColorChanged;
    }
}
