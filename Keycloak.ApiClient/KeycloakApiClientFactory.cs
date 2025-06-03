using keycloak;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Keycloak.ApiClient
{
    public static class KeycloakApiClientFactory
    {
        public static async Task<KeycloakApiClient> GetKeycloakApiClientAsync(string baseUrl, string username, string password)
        {
            var httpClient = await GetHttpClientAsync(baseUrl, username, password);
            var result = new KeycloakApiClient(baseUrl, httpClient);
            return result;
        }

        private static async Task<HttpClient> GetHttpClientAsync(string baseUrl, string username, string password)
        {
            var handler = new HttpClientHandler();
            var httpClient = new HttpClient(handler);
            var bearerToken = await GetAdminTokenAsync(baseUrl, username, password);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            return httpClient;
        }

        private static async Task<string> GetAdminTokenAsync(string baseUrl, string username, string password)
        {
            var client = new HttpClient();
            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/realms/master/protocol/openid-connect/token")
            {
                Content = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("client_id", "admin-cli"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
                new KeyValuePair<string, string>("grant_type", "password")
            })
            };

            var response = await client.SendAsync(tokenRequest);
            response.EnsureSuccessStatusCode();
            var tokenResponse = await response.Content.ReadAsStringAsync();
            var token = ExtractAccessToken(tokenResponse);
            return token;
        }
        private static string ExtractAccessToken(string response)
        {
            var startIndex = response.IndexOf("access_token\":\"") + 15;
            var endIndex = response.IndexOf("\"", startIndex);
            return response.Substring(startIndex, endIndex - startIndex);
        }
    }
}
