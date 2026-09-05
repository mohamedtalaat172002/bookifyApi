namespace Bookify.Infrastructure.Outbox
{
    internal sealed class OutboxOptions
    {
        public int intervalInSeconds { get; init; } //how often the outbox messages will be processed?
        public int Batchsize { get; init; }  // how many messages will be processed in one single run of the background job?
    }
}
