using backend.Models.DTOs.Keycloak;

namespace backend.Models.DTOs
{
    public class TokenResponse
    {
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; } // in seconds
        public string RefreshToken { get; set; }
        public int RefreshExpiresIn { get; set; }
        public string TokenType { get; set; }

        public TokenResponse() { }

        public TokenResponse(KeycloakTokenResponse keycloakTokenResponse)
        {
            AccessToken = keycloakTokenResponse.AccessToken;
            ExpiresIn = keycloakTokenResponse.ExpiresIn;
            RefreshToken = keycloakTokenResponse.RefreshToken;
            RefreshExpiresIn = keycloakTokenResponse.ExpiresIn;
            TokenType = keycloakTokenResponse.TokenType;
        }
    }
}
