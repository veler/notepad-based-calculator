using Microsoft.AspNetCore.Authorization;

namespace NotepadBasedCalculator.WebService.V1.Controllers
{
    [ApiController]
    [Authorize]
    [ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CurrenciesController : ControllerBase
    {
        // GET ~/v1/orders/{accountId}
        [HttpGet("{accountId}")]
        public IActionResult Get(string accountId, ApiVersion apiVersion) =>
            Ok(new Order(GetType().FullName, accountId, apiVersion.ToString()));

        public class Order
        {
            public Order(string controller, string accountId, string apiVersion)
            {
                Controller = controller;
                AccountId = accountId;
                ApiVersion = apiVersion;
            }

            public string Controller { get; set; }

            public string AccountId { get; set; }

            public string ApiVersion { get; set; }
        }
    }
}
