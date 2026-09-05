using Bookify.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Configurations
{
    internal sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMesssage>
    {
        public void Configure(EntityTypeBuilder<OutboxMesssage> builder)
        {
            builder.ToTable("OutboxMessages");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Content).HasColumnType("NVARCHAR(MAX)");
        }
    }
}
