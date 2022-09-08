using System.Net;
using NotepadBasedCalculator.Shared;

namespace NotepadBasedCalculator.Core
{
    [Export]
    internal sealed class WebService
    {
        private readonly IConfigurationReader? _configurationReader;
        private readonly string? _urlBase;

        [ImportingConstructor]
        public WebService(IMefProvider mefProvider)
        {
            try
            {
                _configurationReader = mefProvider.Import<IConfigurationReader>();
                _urlBase = _configurationReader.WebServiceUrl + "api/v1/";
            }
            catch
            {
                // Ignore.
                _configurationReader = null;
            }
        }

        internal async Task<T> GetAsync<T>(string route, object? args = null)
        {
            if (string.IsNullOrEmpty(_urlBase))
            {
                return default(T)!;
            }

            await AuthenticateAsync().ConfigureAwait(true);
            return default(T)!;
        }

        internal Task<T> PostAsync<T>(string route, object? args = null)
        {
            return Task.FromResult(default(T)!);
        }

        private async Task AuthenticateAsync()
        {
            Guard.IsNotNull(_configurationReader);
            Guard.IsNotNull(_urlBase);

            try
            {
                string url = _urlBase + "Authentication/authenticate";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "text/json;charset=UTF-8";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write("\"" + _configurationReader.WebServiceAppId + "\"");
                }

                using var httpResponse = (HttpWebResponse)await httpWebRequest.GetResponseAsync().ConfigureAwait(false);
                using var streamReader = new StreamReader(httpResponse.GetResponseStream());

                string result = streamReader.ReadToEnd();

            }
            catch (Exception ex)
            {

            }
        }
    }
}
