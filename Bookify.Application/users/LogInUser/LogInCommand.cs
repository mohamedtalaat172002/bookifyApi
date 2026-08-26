using Bookify.Application.Abstraction.Messaging;

namespace Bookify.Application.users.LogInUser
{
    public sealed record LogInCommand(string Email, string Password) : ICommand<AccessTokenResponse>;

}
