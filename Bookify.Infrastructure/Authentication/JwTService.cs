using bookify.domain.Abstractions;
using Bookify.Application.Abstraction.Authentication;
using Bookify.Infrastructure.Authentication.Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace Bookify.Infrastructure.Authentication
{
    internal sealed class JwTService : IJwTService
    {
        private readonly HttpClient _httpClient;
        private readonly KeycloakOptions _keycloakOptions;
        private static readonly Error AuthenticationFailed = new(
            "KeyClock.AuthenticationFailed",
            "Failed to get The Token From KeyClock due to Authentication failur ");
        public JwTService(HttpClient httpClient, IOptions<KeycloakOptions> keycloakOptions)
        {
            _httpClient = httpClient;
            _keycloakOptions = keycloakOptions.Value;
        }

        public async Task<Result<string>> GetAccessTokenAsync(string Email, string password, CancellationToken cancellationToken)
        {
            try
            {

                var authRequestParameters = new KeyValuePair<string, string>[]
            {
               new ("client_id",_keycloakOptions.AuthClientId),
               new ("client_secret",_keycloakOptions.AuthClientSecret),
               new ("grant_type","password"),
               new ("scope","openid"),
               new ("username",Email),
               new ("password",password),

            };
                var authorizationRequestContent = new FormUrlEncodedContent(authRequestParameters);
                var response = await _httpClient.PostAsync("", authorizationRequestContent, cancellationToken);
                response.EnsureSuccessStatusCode();
                var authorzationToken = await response.Content.ReadFromJsonAsync<AuthorizationToken>();
                if (authorzationToken == null)
                { return Result.Failure<string>(AuthenticationFailed); }
                return authorzationToken.AccessToken;
            }
            catch (HttpRequestException)
            {
                return Result.Failure<string>(AuthenticationFailed);
            }

        }
    }
}
