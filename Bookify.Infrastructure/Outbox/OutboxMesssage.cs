namespace Bookify.Infrastructure.Outbox
{
    public sealed class OutboxMesssage
    {
        public OutboxMesssage(Guid id, string type, DateTime occuredOnUtc, string content)
        {
            Id = id;
            Type = type;
            OccuredOnUtc = occuredOnUtc;
            Content = content;
        }

        public Guid Id { get; init; }
        public string Type { get; init; }
        public DateTime OccuredOnUtc { get; init; }
        public string Content { get; init; }
        public DateTime? ProcessedOnUtc { get; init; }
        public string? Error { get; init; }

    }
}
