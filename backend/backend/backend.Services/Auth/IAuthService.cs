using backend.Models.DTOs;
using backend.Models.DTOs.Keycloak;

namespace backend.Services.Auth
{
    public interface IAuthService
    {
        public Task<KeycloakTokenResponse> LoginRawAsync(string username, string password);
        public Task<TokenResponse> LoginCustomAsync(string username, string password);
        public Task<KeycloakTokenResponse> ExchangeOidcCodeRawAsync(string code);
        public Task<TokenResponse> ExchangeOidcCodeCustomAsync(string code);
    }
}
