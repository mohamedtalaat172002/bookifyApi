using bookify.domain.Users;

namespace Bookify.Application.Abstraction.Authentication
{
    public interface IAuthenticationService
    {
        Task<String> RegisterAsync(User user, string Password, CancellationToken cancellationToken = default);
    }
}
