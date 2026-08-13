using bookify.domain.Apartments;
using bookify.domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Configurations
{
    internal sealed class ApartmentConfiguration : IEntityTypeConfiguration<Apartment>
    {
        public void Configure(EntityTypeBuilder<Apartment> builder)
        {
            builder.ToTable("Apartments");
            builder.HasKey(a => a.id);
            builder.OwnsOne(a => a.Address);

            builder.Property(a => a.Name).HasMaxLength(200)
                .HasConversion(
                Name => Name.value,
                value => new Name(value));

            builder.Property(a => a.Description).HasMaxLength(2000)
                .HasConversion(
                Description => Description.value,
                value => new Description(value));

            builder.OwnsOne(a => a.Price, priceBuilder =>
            {
                priceBuilder.Property(Money => Money.Currency)
                .HasConversion(Currency => Currency.Code,
                    Code => Currency.FromCode(Code));
            });

            builder.OwnsOne(a => a.CleaningFee, priceBuilder =>
            {
                priceBuilder.Property(Money => Money.Currency)
                .HasConversion(Currency => Currency.Code,
                    Code => Currency.FromCode(Code));
            });

            builder.Property<uint>("Version").IsRowVersion();

        }
    }
}








