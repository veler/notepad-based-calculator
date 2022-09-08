using System.ComponentModel.Composition;
using NotepadBasedCalculator.Shared;

namespace NotepadBasedCalculator.StandaloneConsoleTestApp
{
    [Export(typeof(IConfigurationReader))]
    internal sealed class ConfigurationReader : IConfigurationReader
    {
        private string? _webServiceAppId;
        private string? _webServiceUrl;

        public string WebServiceAppId
        {
            get
            {
                if (!string.IsNullOrEmpty(_webServiceAppId))
                {
                    return _webServiceAppId;
                }

                _webServiceAppId = ReadFile("WebServiceAppId");
                return _webServiceAppId;
            }
        }

        public string WebServiceUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(_webServiceUrl))
                {
                    return _webServiceUrl;
                }

                _webServiceUrl = ReadFile("WebServiceUrl");
                return _webServiceUrl;
            }
        }

        private static string ReadFile(string name)
        {
            string directoryPath = AppDomain.CurrentDomain.BaseDirectory;
#if DEBUG
            string configuration = "Debug";
#else
            string configuration = "Release";
#endif

            string filePath = Path.Combine(directoryPath, $"{name}.{configuration}.txt");
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }

            throw new FileNotFoundException("Unable to find the web service app id file.");
        }
    }
}
