using bookify.domain.Users;
using Bookify.Application.Abstraction.Caching;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure.Authorization
{
    internal sealed class AuthorizationService
    {
        private ApplicationDbContext _dbContext;
        private readonly ICachService _cacheService;
        public AuthorizationService(ApplicationDbContext dbContext, ICachService cacheService)
        {
            this._dbContext = dbContext;
            _cacheService = cacheService;
        }
        public async Task<UserRoleResponse> GetUserRoles(string identityId)
        {

            var cacheKey = $"auth:roles-{identityId}";
            var cachedRoles = await _cacheService.GetAsync<UserRoleResponse>(cacheKey);
            if (cachedRoles is not null)
            {
                return cachedRoles;
            }

            var userRoles = await _dbContext.Set<User>()
                .Where(user => user.IdentityID == identityId)
                .Select(u => new UserRoleResponse
                {
                    Roles = u.Roles.ToList(),
                    UserId = u.id
                }).FirstAsync();

            await _cacheService.SetAsync(cacheKey, userRoles);
            return userRoles;
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
