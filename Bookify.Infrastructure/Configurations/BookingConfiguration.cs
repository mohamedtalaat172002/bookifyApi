using bookify.domain.Apartments;
using bookify.domain.Bookings;
using bookify.domain.Shared;
using bookify.domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Configurations
{
    internal sealed class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.ToTable("Bookings");
            builder.HasKey(b => b.id);
            builder.OwnsOne(b => b.PriceForPeriod, Pricebuilder =>
            {
                Pricebuilder.Property(p => p.Currency).HasConversion
                (Currency => Currency.Code, code => Currency.FromCode(code));
            });

            builder.OwnsOne(builder => builder.CleaningFee, cleaningFeeBuilder =>
            cleaningFeeBuilder.Property(c => c.Currency)
            .HasConversion(Currency => Currency.Code, Code => Currency.FromCode(Code)));

            builder.OwnsOne(builder => builder.AmenetiesUpCharge, amenetiesUpChargeBuilder =>
            {
                amenetiesUpChargeBuilder.Property(a => a.Currency).HasConversion
                (Currency => Currency.Code, code => Currency.FromCode(code));
            });

            builder.OwnsOne(builder => builder.TotalPrice, totalPriceBuilder =>
            {
                totalPriceBuilder.Property(a => a.Currency).HasConversion
                (Currency => Currency.Code, code => Currency.FromCode(code));
            });

            builder.OwnsOne(builder => builder.Duration);

            builder.HasOne<Apartment>()
            .WithMany().HasForeignKey(booking => booking.ApartmentId);

            builder.HasOne<User>()
                .WithMany().HasForeignKey(booking => booking.UserId);

        }
    }
}
