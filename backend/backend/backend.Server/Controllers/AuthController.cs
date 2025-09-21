using backend.Models.DTOs;
using backend.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace backend.Server.Controllers
{
    [Route("[controller]")]
    public class AuthController : ApiController
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // Username/password → raw KC tokens
        [HttpPost("login/raw")]
        public async Task<IActionResult> LoginRawAsync([FromBody] LoginRequest request)
        {
            var tokenResponse = await _authService.LoginRawAsync(request.Username, request.Password);
            return Ok(tokenResponse);
        }

        // Username/password → custom app tokens
        [HttpPost("login/custom")]
        public async Task<IActionResult> LoginCustomAsync([FromBody] LoginRequest request)
        {
            var tokenResponse = await _authService.LoginCustomAsync(request.Username, request.Password);
            return Ok(tokenResponse);
        }

        // OIDC code exchange → raw KC tokens
        [HttpPost("oidc-exchange-raw")]
        public async Task<IActionResult> LoginRawOidcAsync([FromBody] OidcExchangeRequest request)
        {
            var tokenResponse = await _authService.ExchangeOidcCodeRawAsync(request.Code);
            return Ok(tokenResponse);
        }

        // OIDC code exchange → custom app tokens
        [HttpPost("oidc-exchange-custom")]
        public async Task<IActionResult> LoginCustomOidcAsync([FromBody] OidcExchangeRequest request)
        {
            var tokenResponse = await _authService.ExchangeOidcCodeCustomAsync(request.Code);
            return Ok(tokenResponse);
        }
    }
}
