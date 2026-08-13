using bookify.domain.Apartments;
using bookify.domain.Bookings;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure.Repositories
{
    internal sealed class BookingRepository : Repository<Booking>, IBookingRepository
    {
        private static readonly BookingStatus[] ActiveBookingStatus =
            {
            BookingStatus.confirmed,
            BookingStatus.reserved,
            BookingStatus.completed,
            };
        public BookingRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }

        public async Task<bool> IsOverlappingAsync(Apartment apartment, DateRange duration, CancellationToken cancellationToken = default)
        {
            return await _applicationDbContext.Set<Booking>().AnyAsync
                (b => b.ApartmentId == apartment.id && b.Duration.Start <= duration.End && b.Duration.End >= duration.Start
                && ActiveBookingStatus.Contains(b.Status), cancellationToken);

        }
    }
}
