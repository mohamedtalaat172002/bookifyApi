using bookify.domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Configurations
{
    internal sealed class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.ToTable("RolePermissions");
            builder.HasKey(rolePermission => new { rolePermission.RoleId, rolePermission.PermissionId });
            builder.HasData(new RolePermission
            {
                PermissionId = Permission.UserRead.Id,
                RoleId = Role.Registered.Id

            });

        }
    }
}
