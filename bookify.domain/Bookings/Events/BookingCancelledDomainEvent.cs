using bookify.domain.Abstractions;

namespace bookify.domain.Bookings.Events
{
    public sealed record class BookingCancelledDomainEvent(Guid id) : IDomainEvent
    {
    }
}
