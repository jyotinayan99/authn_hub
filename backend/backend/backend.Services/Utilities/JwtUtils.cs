using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Newtonsoft.Json;

public static class JwtUtils
{
    public const string SecretKey = "Vm4W-peqk4bhZbAP0km0eEhyc8uLgcWub9ng-YJp9EjolWSqtY7FX2-7KNZi4aplOLjGVpXJiXIul_oQf0Di6y4BEP6A8ZCvxnKtynTRVOGeRkie77HCff_60Cn_sdvokMJvu0HCmOvz0QddAvTUl6T-Cscp7u8c20OlsblcY8ge75L2dPXlkuqkg07XEk-4pim6mGup4p-u_WPUie1aI9VMKvo6nKADnvJfzawClI191TB1ylIHoMnzrQe6JCFJRMWYt2wDGOzpeIt0prjRDNHB4bopZ8nP_OzLjx0nE7W3pF4yAEEWKcT-OKroeBqmFH0dkc2rq2395Wq3M-wAkA";
    public const int ExpiresIn = 5;

    /// <summary>
    /// Decodes a JWT token payload into a typed object.
    /// </summary>
    public static T? DecodeJwt<T>(string jwtToken)
    {
        var handler = new JwtSecurityTokenHandler();

        if (!handler.CanReadToken(jwtToken))
            throw new ArgumentException("Invalid JWT token", nameof(jwtToken));

        var token = handler.ReadJwtToken(jwtToken);

        // JWT payload is in dictionary form
        var payloadJson = JsonConvert.SerializeObject(token.Payload);

        // Deserialize into the provided type
        return JsonConvert.DeserializeObject<T>(payloadJson);
    }

    public static string GenerateJwt(Dictionary<string, object> claimsDict, int expiresIn)
    {
        var claims = new List<Claim>();
        foreach (var kv in claimsDict)
        {
            claims.Add(new Claim(kv.Key, kv.Value?.ToString() ?? string.Empty));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "your-app",         // optional
            audience: "your-app-client",// optional
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiresIn),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
