using bookify.domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure.Authorization
{
    internal sealed class AuthorizationService
    {
        private ApplicationDbContext _dbContext;

        public AuthorizationService(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task<UserRoleResponse> GetUserRoles(string identityId)
        {
            return await _dbContext.Set<User>()
                .Where(user => user.IdentityID == identityId)
                .Select(u => new UserRoleResponse
                {
                    Roles = u.Roles.ToList(),
                    UserId = u.id
                }).FirstAsync();

        }
    }
}
