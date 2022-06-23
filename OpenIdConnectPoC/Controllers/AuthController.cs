using Microsoft.AspNetCore.Mvc;
using OpenIdConnectPoC.Services;

namespace OpenIdConnectPoC.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly AuthService _googleAuthService;
        public AuthController(AuthService googleAuthService)
        {
            _googleAuthService = googleAuthService;
        }

        [HttpGet]
        public IActionResult Authorise()
        {
            return _googleAuthService.Authorise();
        }

        [HttpGet("handle-authorisation")]
        public async Task<IActionResult> HandleAuthorisation()
        {
            await _googleAuthService.HandleAuthorisation();
            return Redirect("/");
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _googleAuthService.Logout();
            return Ok();
        }

        public record IsAuthenticatedResult(bool IsAuthenticated);
        [HttpGet("is-authenticated")]
        public IActionResult IsAuthenticated()
        {
            return Ok(new IsAuthenticatedResult(this.User?.Identity?.IsAuthenticated ?? false));
        }
    }
}
