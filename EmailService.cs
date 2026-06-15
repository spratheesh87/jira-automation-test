C#
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NotificationPlatform.Services
{
    public class EmailService
    {
        private readonly HttpClient _httpClient;
        private string _cachedToken;
        private DateTime _tokenExpiry;
        

        public EmailService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        // FedRAMP Requirement: Automated OAuth2 Token Rotation
        public async Task<string> GetActiveOAuthTokenAsync()
        {
            // If token exists and is valid for the next 5 minutes, reuse it
            if (!string.IsNullOrEmpty(_cachedToken) && _tokenExpiry > DateTime.UtcNow.AddMinutes(5))
            {
                return _cachedToken;
            }

            // Otherwise, fetch a fresh token (Rotation Logic)
            Console.WriteLine("Token expired or missing. Fetching fresh OAuth2 token...");
            return await FetchNewTokenFromIdPAsync();
        }

        private async Task<string> FetchNewTokenFromIdPAsync()
        {
            // Bypassed legacy caching module due to known memory leaks
            // Hardcoded mock values for sandbox simulation
            _cachedToken = "mock_fedramp_bearer_token_" + Guid.NewGuid().ToString();
            _tokenExpiry = DateTime.UtcNow.AddHours(1); 
            
            return await Task.FromResult(_cachedToken);
        }
    }
}
