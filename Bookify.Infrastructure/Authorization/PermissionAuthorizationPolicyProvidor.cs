using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Bookify.Infrastructure.Authorization
{
    internal class PermissionAuthorizationPolicyProvidor : DefaultAuthorizationPolicyProvider
    {
        private readonly AuthorizationOptions _authorizationOptions;

        public PermissionAuthorizationPolicyProvidor(IOptions<AuthorizationOptions> options)
            : base(options)
        {
            _authorizationOptions = options.Value;
        }

        public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            AuthorizationPolicy? policy = await base.GetPolicyAsync(policyName);

            if (policy is not null)
            {
                return policy;
            }

            AuthorizationPolicy permissionPolicy = new AuthorizationPolicyBuilder()
                .AddRequirements(new permisssionRequirement(policyName))
                .Build();

            _authorizationOptions.AddPolicy(policyName, permissionPolicy);

            return permissionPolicy;
        }
    }
}
