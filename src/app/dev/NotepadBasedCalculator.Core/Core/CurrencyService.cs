namespace NotepadBasedCalculator.Core
{
    [Export(typeof(ICurrencyService))]
    [Shared]
    internal sealed class CurrencyService : ICurrencyService
    {
        private readonly WebService _webService;

        [ImportingConstructor]
        public CurrencyService(WebService webService)
        {
            _webService = webService;
            _webService.GetAsync<object>("", "");
        }
    }
}
