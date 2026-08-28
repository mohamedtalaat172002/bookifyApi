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

        public async Task<HashSet<string>> GetUserPermissions(string identityId)
        {
            var permissions = await _dbContext.Set<User>()
                .Where(u => u.IdentityID == identityId)
                .SelectMany(u => u.Roles.Select(r => r.permissions))
                .FirstAsync();
            var PermissionHahsed = permissions.Select(p => p.Name).ToHashSet();
            return PermissionHahsed;
        }
    }
}
