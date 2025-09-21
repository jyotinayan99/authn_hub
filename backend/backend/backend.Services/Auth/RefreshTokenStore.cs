using System.Collections.Concurrent;

namespace backend.Services.Auth
{
    public interface IRefreshTokenStore
    {
        void SaveAsync(string appRefreshToken, string kcRefreshToken, string userId);
        string? GetKeycloakRefreshTokenAsync(string appRefreshToken);
        void RemoveAsync(string appRefreshToken);
    }

    public class RefreshTokenStore : IRefreshTokenStore
    {
        private readonly ConcurrentDictionary<string, (string KcRefreshToken, string UserId)> _refreshTokenStore
            = new ConcurrentDictionary<string, (string, string)>();

        public void SaveAsync(string appRefreshToken, string kcRefreshToken, string userId)
        {
            _refreshTokenStore[appRefreshToken] = (kcRefreshToken, userId);
        }

        public string? GetKeycloakRefreshTokenAsync(string appRefreshToken)
        {
            if (_refreshTokenStore.TryGetValue(appRefreshToken, out var entry))
                return entry.KcRefreshToken;

            return null;
        }

        public void RemoveAsync(string appRefreshToken)
        {
            _refreshTokenStore.TryRemove(appRefreshToken, out _);
        }
    }
}
