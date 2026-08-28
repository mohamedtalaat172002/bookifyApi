using Microsoft.AspNetCore.Authorization;

namespace Bookify.Infrastructure.Authorization
{
    internal sealed class permisssionRequirement : IAuthorizationRequirement
    {
        public permisssionRequirement(string permissison)
        {
            this.permisssion = permissison;
        }
        public string permisssion { get; set; }
    }
}
