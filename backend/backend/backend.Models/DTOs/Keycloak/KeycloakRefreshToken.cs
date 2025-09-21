using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.Models.DTOs.Keycloak
{
    public class KeycloakRefreshToken
    {
        public string Jti { get; set; }            // Unique token identifier
        public string Sub { get; set; }            // User ID (subject)
        public string Typ { get; set; }            // Token type (usually "Refresh")
        public string Iss { get; set; }            // Issuer (Keycloak base URL + realm)
        public string Aud { get; set; }            // Audience (clientId)
        public long Iat { get; set; }              // Issued at (epoch seconds)
        public long Exp { get; set; }              // Expiration time (epoch seconds)
        public string Sid { get; set; }            // Session ID
        public string ClientId { get; set; }       // The requesting client
        public string SessionState { get; set; }   // KC session state
        public string Scope { get; set; }          // Scopes granted
        public string Username { get; set; }       // Username (sometimes included)

        // Optionally add others if your realm includes them
        public string Email { get; set; }
        public string Name { get; set; }
    }
}
