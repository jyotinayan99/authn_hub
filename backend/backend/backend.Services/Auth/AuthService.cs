using backend.Models.DTOs;
using backend.Models.DTOs.Keycloak;

namespace backend.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IKeycloakService _keycloakService;
        private readonly IRefreshTokenStore _refreshTokenStore;

        public AuthService(IKeycloakService keycloakService, IRefreshTokenStore refreshTokenStore)
        {
            _keycloakService = keycloakService;
            _refreshTokenStore = refreshTokenStore;
        }

        // ------------------------
        // Username/password flows
        // ------------------------
        public async Task<KeycloakTokenResponse> LoginRawAsync(string username, string password)
        {
            return await _keycloakService.GetTokenAsync(username, password);
        }

        public async Task<TokenResponse> LoginCustomAsync(string username, string password)
        {
            var kcResponse = await _keycloakService.GetTokenAsync(username, password);
            return GenerateCustomTokens(kcResponse);
        }

        // ------------------------
        // OIDC code exchange flows
        // ------------------------
        public async Task<KeycloakTokenResponse> ExchangeOidcCodeRawAsync(string code)
        {
            return await _keycloakService.ExchangeOidcCodeAsync(code);
        }

        public async Task<TokenResponse> ExchangeOidcCodeCustomAsync(string code)
        {
            var kcResponse = await _keycloakService.ExchangeOidcCodeAsync(code);
            return GenerateCustomTokens(kcResponse);
        }

        // ------------------------
        // Internal helper
        // ------------------------
        private TokenResponse GenerateCustomTokens(KeycloakTokenResponse kcResponse)
        {
            var kcAccessToken = JwtUtils.DecodeJwt<KeycloakAccessToken>(kcResponse.AccessToken);

            var accessClaims = new Dictionary<string, object>
            {
                { "sub", kcAccessToken?.Sub ?? string.Empty },
                { "username", kcAccessToken?.PreferredUsername ?? string.Empty },
                { "email", kcAccessToken?.Email ?? string.Empty },
                { "roles", kcAccessToken?.RealmRoles != null
                            ? string.Join(",", kcAccessToken.RealmRoles)
                            : string.Empty }
            };
            var appAccessToken = JwtUtils.GenerateJwt(accessClaims, 5);

            var refreshClaims = new Dictionary<string, object>
            {
                { "sub", kcAccessToken?.Sub ?? string.Empty },
                { "tokenType", "refresh" }
            };
            var appRefreshToken = JwtUtils.GenerateJwt(refreshClaims, 30);

            _refreshTokenStore.SaveAsync(appRefreshToken, kcResponse.RefreshToken, kcAccessToken?.Sub ?? string.Empty);

            return new TokenResponse
            {
                AccessToken = appAccessToken,
                RefreshToken = appRefreshToken,
                ExpiresIn = 300,
                RefreshExpiresIn = 1800,
                TokenType = "Bearer"
            };
        }
    }
}
