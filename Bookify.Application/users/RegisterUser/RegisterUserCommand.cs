using Bookify.Application.Abstraction.Messaging;

namespace Bookify.Application.users.RegisterUser
{
    public sealed record RegisterUserCommand(string Email, string FirstName, string LastName, string Password) : ICommand<Guid>;

}
