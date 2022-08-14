namespace NotepadBasedCalculator.Api
{
    /// <summary>
    /// Provides a way to import MEF components on the fly.
    /// </summary>
    public interface IMefProvider : IServiceProvider
    {
        /// <summary>
        /// Imports the given type.
        /// </summary>
        TExport Import<TExport>();

        /// <summary>
        /// Imports the given type.
        /// </summary>
        IEnumerable<TExport> ImportMany<TExport>();
    }
}
