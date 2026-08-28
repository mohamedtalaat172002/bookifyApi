using bookify.domain.Users;

namespace Bookify.Infrastructure.Authorization
{
    internal sealed class UserRoleResponse
    {
        public Guid UserId { get; init; }

        public List<Role> Roles { get; init; }
    }
}
