using bookify.domain.Apartments;
using bookify.domain.Bookings;
using bookify.domain.Reviews;
using bookify.domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Configurations
{
    internal sealed class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("Reviews");

            builder.HasKey(r => r.id);

            builder.Property(r => r.Rating)
                .HasConversion(
                    rating => rating.Value,
                    value => Rating.Create(value).Value
                ).IsRequired();

            builder.Property(r => r.Comment).HasMaxLength(200)
                .HasConversion
                (r => r.Value,
                value => new Comment(value));

            builder.HasOne<Apartment>()
                .WithMany()
                .HasForeignKey(r => r.ApartmentId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne<Booking>()
                .WithMany()
                .HasForeignKey(r => r.BookingId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
