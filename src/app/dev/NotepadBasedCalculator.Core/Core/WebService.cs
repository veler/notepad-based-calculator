using System.Net;
using System.Net.Http;
using System.Text;
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

#pragma warning disable IDE0060 // Remove unused parameter
        internal async Task<T> GetAsync<T>(string route, object? args = null)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            if (string.IsNullOrEmpty(_urlBase))
            {
                return default!;
            }

            await AuthenticateAsync().ConfigureAwait(true);
            return default!;
        }

#pragma warning disable CA1822 // Can be marked as static
#pragma warning disable IDE0060 // Remove unused parameter
        internal Task<T> PostAsync<T>(string route, object? args = null)
#pragma warning restore IDE0060 // Remove unused parameter
#pragma warning restore CA1822 // Can be marked as static
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

                using var httpClient = new HttpClient();
                using HttpResponseMessage httpResponse
                      = await httpClient.PostAsync(
                          url,
                          new StringContent(
                              "\"" + _configurationReader.WebServiceAppId + "\"",
                              Encoding.UTF8,
                              "text/json;charset=UTF-8"));

                string result = await httpResponse.Content.ReadAsStringAsync();

            }
            catch
            {

            }
        }
    }
}
