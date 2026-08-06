using bookify.domain.Abstractions;

namespace bookify.domain.Users.Events
{
    public sealed record UserCreatedDomainEvents(Guid userId) : IDomainEvent
    {
    }
}
