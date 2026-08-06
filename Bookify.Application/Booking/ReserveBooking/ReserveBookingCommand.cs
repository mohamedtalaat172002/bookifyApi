using Bookify.Application.Abstraction.Messaging;

namespace Bookify.Application.Booking.ReserveBooking
{

    public record ReserveBookingCommand(Guid ApartmentId, Guid UserId, DateTime StartDate, DateTime EndDate
        ) : ICommand<Guid>
    {

    }
}
