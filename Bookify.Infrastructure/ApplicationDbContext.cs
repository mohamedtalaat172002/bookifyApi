using bookify.domain.Abstractions;
using Bookify.Application.Abstraction.Clock;
using Bookify.Application.Exceptions;
using Bookify.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Bookify.Infrastructure
{
    public sealed class ApplicationDbContext : DbContext, IUniteOfWork
    {
        private static readonly JsonSerializerSettings jsonSerializerSettings = new()
        {
            TypeNameHandling = TypeNameHandling.All
        };
        private readonly IDateTimeProvider _dateTimeProvider;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDateTimeProvider dateTimeProvider) : base(options)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            try
            {
                AddDomainEventsAsOutboxMessages();

                var result = await base.SaveChangesAsync(cancellationToken);
                return result;
            }
            catch (ConcurrencyException ex)
            {
                throw new ConcurrencyException("Concurrency Exception Occured", ex);
            }
        }

        public void AddDomainEventsAsOutboxMessages()
        {
            var outBoxMessages = ChangeTracker.Entries<Entity>()
                  .Select(e => e.Entity)
                  .SelectMany(e =>
                  {
                      var domainEvents = e.GetEvents();
                      e.ClearDomainEvents();
                      return domainEvents;
                  })
                  .Select(domainEvent => new OutboxMesssage(
                      Guid.NewGuid(),
                        domainEvent.GetType().Name,
                          _dateTimeProvider.UtcNow,
                        JsonConvert.SerializeObject(domainEvent, jsonSerializerSettings)
                  )).ToList();

            AddRange(outBoxMessages);


        }

    }
}
