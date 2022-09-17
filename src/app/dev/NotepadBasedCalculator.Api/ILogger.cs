namespace NotepadBasedCalculator.Api
{
    /// <summary>
    /// Provides a way to log information for debugging purpose.
    /// </summary>
    public interface ILogger
    {
        void Log(string logName, params ValueTuple<string, string?>[]? properties);

        void Log(string logName, string? description, params ValueTuple<string, string?>[]? properties);

        void LogFault(string logName, Exception ex, params ValueTuple<string, object?>[]? properties);

        void LogFault(string logName, Exception ex, string description, params ValueTuple<string, object?>[]? properties);
    }
}
