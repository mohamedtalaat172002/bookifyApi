using Bookify.Application.Abstraction.Messaging;

namespace Bookify.Application.Booking.GetBooking
{
    public record GetBookingQuery(Guid id) : IQuery<BookingResponse>
    {
    }
}
