using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Security.Authentication;
using backend.Models.DTOs.Keycloak;

public interface IKeycloakService
{
    Task<KeycloakTokenResponse> GetTokenAsync(string username, string password);
    Task<KeycloakTokenResponse> ExchangeOidcCodeAsync(string code);
}

public class KeycloakService : IKeycloakService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly string _realm;
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly string _redirectUri;

    public KeycloakService(IConfiguration config)
    {
        _httpClient = new HttpClient();

        // Load Keycloak config once
        _baseUrl = config["Keycloak:BaseUrl"] ?? throw new ArgumentNullException("Keycloak:BaseUrl");
        _realm = config["Keycloak:Realm"] ?? throw new ArgumentNullException("Keycloak:Realm");
        _clientId = config["Keycloak:ClientId"] ?? throw new ArgumentNullException("Keycloak:ClientId");
        _clientSecret = config["Keycloak:ClientSecret"] ?? throw new ArgumentNullException("Keycloak:ClientSecret");
        _redirectUri = config["Keycloak:RedirectUri"] ?? throw new ArgumentNullException("Keycloak:RedirectUri");
    }

    public async Task<KeycloakTokenResponse> GetTokenAsync(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username cannot be empty", nameof(username));

        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be empty", nameof(password));

        var content = new FormUrlEncodedContent(
        [
            new KeyValuePair<string, string>("client_id", _clientId),
            new KeyValuePair<string, string>("client_secret", _clientSecret),
            new KeyValuePair<string, string>("grant_type", "password"),
            new KeyValuePair<string, string>("username", username),
            new KeyValuePair<string, string>("password", password)
        ]);

        var tokenEndpoint = $"{_baseUrl}/realms/{_realm}/protocol/openid-connect/token";
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.PostAsync(tokenEndpoint, content);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to connect to Keycloak", ex);
        }

        if (!response.IsSuccessStatusCode)
            throw new AuthenticationException("Invalid username or password");

        var json = await response.Content.ReadAsStringAsync();

        KeycloakTokenResponse tokenResponse;
        try
        {
            tokenResponse = JsonConvert.DeserializeObject<KeycloakTokenResponse>(json)
                            ?? throw new Exception("Failed to parse Keycloak response");
        }
        catch (JsonException ex)
        {
            throw new Exception("Invalid JSON received from Keycloak", ex);
        }

        return tokenResponse;
    }

    public async Task<KeycloakTokenResponse> ExchangeOidcCodeAsync(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Code cannot be empty", nameof(code));

        var content = new FormUrlEncodedContent(
        [
            new KeyValuePair<string, string>("client_id", _clientId),
            new KeyValuePair<string, string>("client_secret", _clientSecret),
            new KeyValuePair<string, string>("grant_type", "authorization_code"),
            new KeyValuePair<string, string>("code", code),
            new KeyValuePair<string, string>("redirect_uri", _redirectUri)
        ]);

        var tokenEndpoint = $"{_baseUrl}/realms/{_realm}/protocol/openid-connect/token";
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.PostAsync(tokenEndpoint, content);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to connect to Keycloak", ex);
        }

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync();
            throw new AuthenticationException($"Keycloak token endpoint returned {response.StatusCode}: {body}");
        }

        var json = await response.Content.ReadAsStringAsync();

        KeycloakTokenResponse tokenResponse;
        try
        {
            tokenResponse = JsonConvert.DeserializeObject<KeycloakTokenResponse>(json)
                            ?? throw new Exception("Failed to parse Keycloak response");
        }
        catch (JsonException ex)
        {
            throw new Exception("Invalid JSON received from Keycloak", ex);
        }

        return tokenResponse;
    }

}
