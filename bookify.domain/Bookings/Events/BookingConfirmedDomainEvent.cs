using bookify.domain.Abstractions;

namespace bookify.domain.Bookings.Events
{
    public sealed record BookingConfirmedDomainEvent(Guid id) : IDomainEvent
    {
    }
}
