namespace NotepadBasedCalculator.Core
{
    [Export(typeof(ILogger))]
    internal sealed class Logger : ILogger
    {
        public void Log(string logName, params (string, string?)[]? properties)
        {
            // TODO
        }

        public void Log(string logName, string? description, params (string, string?)[]? properties)
        {
            // TODO
        }

        public void LogFault(string logName, Exception ex, params (string, object?)[]? properties)
        {
            // TODO
        }

        public void LogFault(string logName, Exception ex, string description, params (string, object?)[]? properties)
        {
            // TODO
        }
    }
}
