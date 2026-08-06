using bookify.domain.Abstractions;

namespace bookify.domain.Bookings.Events
{
    public sealed record class BookingCompletedDomainEvent(Guid id) : IDomainEvent
    {
    }
}
