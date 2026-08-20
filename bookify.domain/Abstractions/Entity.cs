namespace bookify.domain.Abstractions
{
    public abstract class Entity
    {
        public List<IDomainEvent> domainEvents = new();
        protected Entity(Guid id)
        {
            this.id = id;
        }
        protected Entity()
        {

        }
        public Guid id { get; init; }

        public IReadOnlyList<IDomainEvent> GetEvents() => domainEvents;
        protected void RaiseDomainEvent(IDomainEvent domainEvent) => domainEvents.Add(domainEvent);

        public void ClearDomainEvents() => domainEvents.Clear();
    }
}
