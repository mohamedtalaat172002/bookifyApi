using bookify.domain.Users;

namespace Bookify.Application.Abstraction.Authentication
{
    public interface IAuthenticationService
    {
        Task<String> RegisterASync(User user, string Password, CancellationToken cancellationToken = default);
    }
}
