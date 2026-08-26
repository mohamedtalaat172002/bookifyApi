using bookify.domain.Users;
using Bookify.Application.Abstraction.Authentication;
using Bookify.Infrastructure.Authentication.Models;
using System.Net.Http.Json;

namespace Bookify.Infrastructure.Authentication
{
    internal sealed class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _httpClient;
        private const string passwordCredentialType = "password";

        public AuthenticationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<string> RegisterAsync(User user, string Password, CancellationToken cancellationToken = default)
        {
            var userRepresentationModel = UserRepresentationModel.FromUser(user);

            userRepresentationModel.Credentials = new CredentialRepresentationModel[]
            {
            new()
            {
                Value = Password,
                Temporary = false,
                Type = passwordCredentialType
            }
            };

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(
                "users",
                userRepresentationModel,
                cancellationToken);

            return ExtractIdentityIdFromLocationHeader(response);
        }

        private static string ExtractIdentityIdFromLocationHeader(
       HttpResponseMessage httpResponseMessage)
        {
            const string usersSegmentName = "users/";

            string? locationHeader = httpResponseMessage.Headers.Location?.PathAndQuery;

            if (locationHeader is null)
            {
                throw new InvalidOperationException("Location header can't be null");
            }

            int userSegmentValueIndex = locationHeader.IndexOf(
                usersSegmentName,
                StringComparison.InvariantCultureIgnoreCase);

            string userIdentityId = locationHeader.Substring(
                userSegmentValueIndex + usersSegmentName.Length);

            return userIdentityId;
        }



    }
}
