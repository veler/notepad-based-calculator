using NotepadBasedCalculator.Shared;

namespace NotepadBasedCalculator.WebService
{
    public class ConfigurationReader : IConfigurationReader
    {
        private string? _webServiceAppId;

        public string WebServiceAppId
        {
            get
            {
                if (!string.IsNullOrEmpty(_webServiceAppId))
                {
                    return _webServiceAppId;
                }

                string directoryPath = AppDomain.CurrentDomain.BaseDirectory;
#if DEBUG
                string configuration = "Debug";
#else
                string configuration = "Release";
#endif

                string filePath = Path.Combine(directoryPath, $"WebServiceAppId.{configuration}.txt");
                if (File.Exists(filePath))
                {
                    _webServiceAppId = File.ReadAllText(filePath);
                    return _webServiceAppId;
                }

                throw new FileNotFoundException("Unable to find the web service app id file.");
            }
        }

        public string WebServiceUrl => throw new NotImplementedException();
    }
}
