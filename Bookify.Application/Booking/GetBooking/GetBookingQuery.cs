using Bookify.Application.Abstraction.Caching;

namespace Bookify.Application.Booking.GetBooking
{
    public record GetBookingQuery(Guid id) : ICachedQuery<BookingResponse>
    {
        public string cacheKey => $"booking:{id}";

        public TimeSpan? cacheExpiration => null;
    }
}
