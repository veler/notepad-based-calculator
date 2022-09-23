namespace NotepadBasedCalculator.Desktop.Platform
{
    /// <summary>
    /// Provides an entry point for the app to start-up anything platform-specific.
    /// </summary>
    internal interface IPlatformInitializer
    {
        /// <summary>
        /// Initializes some platform-specific work.
        /// </summary>
        void Initialize();
    }
}
