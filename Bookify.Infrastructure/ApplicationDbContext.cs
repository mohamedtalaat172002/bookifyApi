using bookify.domain.Abstractions;
using Bookify.Application.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure
{
    public sealed class ApplicationDbContext : DbContext, IUniteOfWork
    {
        private readonly IPublisher _publisher;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IPublisher publisher) : base(options)
        {
            _publisher = publisher;
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
                var result = await base.SaveChangesAsync(cancellationToken);
                await PublishDomainEventsAsync();
                return result;
            }
            catch (ConcurrencyException ex)
            {
                throw new ConcurrencyException("Concurrency Exception Occured", ex);
            }
        }

        public async Task PublishDomainEventsAsync()
        {
            var domainEvents = ChangeTracker.Entries<Entity>()
                  .Select(e => e.Entity)
                  .SelectMany(e =>
                  {
                      var domainEvents = e.GetEvents();
                      e.ClearDomainEvents();
                      return domainEvents;
                  });

            foreach (var domainEvent in domainEvents)
            {
                await _publisher.Publish(domainEvent);
            }

        }

    }
}
