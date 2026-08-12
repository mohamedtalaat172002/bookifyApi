using bookify.domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Configurations
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.id);

            builder.Property(u => u.FirstName)
                .HasMaxLength(100)
                .HasConversion(
                    firstName => firstName.value,
                    value => new FirstName(value));

            builder.Property(u => u.LastName).HasMaxLength(100)

                .HasConversion(
                    lastName => lastName.value,
                    value => new LastName(value));

            builder.Property(u => u.Email).HasMaxLength(100)

                .HasConversion(
                    email => email.value,
                    value => new bookify.domain.Users.Email(value));

            builder.HasIndex(user => user.Email).IsUnique();



        }
    }
}
