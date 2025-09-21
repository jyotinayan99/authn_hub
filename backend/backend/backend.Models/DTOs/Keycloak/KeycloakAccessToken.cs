namespace backend.Models.DTOs.Keycloak
{
    public class KeycloakAccessToken
    {
        public string Jti { get; set; }              // unique token id
        public string Iss { get; set; }              // issuer (KC realm url)
        public string Sub { get; set; }              // subject (user id)
        public string Typ { get; set; }              // token type (Bearer)
        public string Azp { get; set; }              // authorized party (client id)
        public string SessionState { get; set; }     // KC session id
        public string PreferredUsername { get; set; }
        public string Email { get; set; }

        public List<string> RealmRoles { get; set; }
        public Dictionary<string, List<string>> ResourceAccess { get; set; }

        public long Exp { get; set; }                // expiry (epoch seconds)
        public long Iat { get; set; }                // issued at
        public long AuthTime { get; set; }           // authentication time
    }

}
