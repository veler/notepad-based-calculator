using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using NotepadBasedCalculator.Shared;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NotepadBasedCalculator.WebService.V1.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfigurationReader _configurationReader;

        public AuthenticationController(IConfigurationReader configurationReader)
        {
            _configurationReader = configurationReader;
        }

        // POST ~/api/v1/authentication/authenticate
        [HttpPost("authenticate")]
        [AllowAnonymous]
        public IActionResult Authenticate([FromBody] string appId)
        {
            if (string.IsNullOrWhiteSpace(appId))
            {
                return BadRequest("App id is missing.");
            }

            if (string.Equals(_configurationReader.WebServiceAppId, appId, StringComparison.Ordinal))
            {
                var secretKey
                    = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            ConfigurationManager.AppSetting["JWT:Secret"]));

                var signinCredentials
                    = new SigningCredentials(
                        secretKey,
                        SecurityAlgorithms.HmacSha256);

                var tokeOptions
                    = new JwtSecurityToken(
                        issuer: ConfigurationManager.AppSetting["JWT:ValidIssuer"],
                        audience: ConfigurationManager.AppSetting["JWT:ValidAudience"],
                        claims: new List<Claim>(),
                        expires: DateTime.UtcNow.AddMinutes(int.Parse(ConfigurationManager.AppSetting["JWT:TokenValidityInMinutes"])),
                        signingCredentials: signinCredentials);

                string tokenString
                    = new JwtSecurityTokenHandler()
                    .WriteToken(tokeOptions);

                return Ok(tokenString);
            }

            return Unauthorized();
        }
    }
}
