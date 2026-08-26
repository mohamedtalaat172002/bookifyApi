namespace Bookify.Application.Abstraction.Authentication
{
    public interface IJwTService
    {
        public Task<bookify.domain.Abstractions.Result<string>> GetAccessTokenAsync(string Email, string password, CancellationToken cancellationToken);
    }
}
