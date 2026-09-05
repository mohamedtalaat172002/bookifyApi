using bookify.domain.Abstractions;
using Bookify.Application.Abstraction.Clock;
using Bookify.Application.Abstraction.Data;
using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;

namespace Bookify.Infrastructure.Outbox
{
    [DisallowConcurrentExecution]
    internal sealed class ProccessOutBoxMessageJob : IJob
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings = new()
        {
            TypeNameHandling = TypeNameHandling.All
        };

        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly IPublisher _publisher;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly OutboxOptions _outboxOptions;
        private readonly ILogger<ProccessOutBoxMessageJob> _logger;

        public ProccessOutBoxMessageJob(ISqlConnectionFactory sqlConnectionFactory, IPublisher publisher, IDateTimeProvider dateTimeProvider, OutboxOptions outboxOptions, ILogger<ProccessOutBoxMessageJob> logger)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _publisher = publisher;
            _dateTimeProvider = dateTimeProvider;
            _outboxOptions = outboxOptions;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation($"Processing outbox messages at {_dateTimeProvider.UtcNow}");
            using var scope = _sqlConnectionFactory.CreateConnection();
            using var transaction = scope.BeginTransaction();
            var outBoxMessages = await GetOutBoxMessages(scope, transaction);
            foreach (var outBoxMessage in outBoxMessages)
            {
                Exception? exception = null;
                try
                {
                    var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(outBoxMessage.content, JsonSerializerSettings);

                    await _publisher.Publish(domainEvent, cancellationToken: context.CancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error processing outbox message with id {outBoxMessage.Id}");
                    exception = ex;
                }
                await updateOutboxMessageAsync(scope, transaction, outBoxMessage, exception);

            }
            transaction.Commit();
            _logger.LogInformation($"Completed Processing {outBoxMessages.Count} outbox messages at {_dateTimeProvider.UtcNow}");

        }



        internal sealed record OutboxMessageResponse(Guid Id, string content);
        private async Task<List<OutboxMessageResponse>> GetOutBoxMessages(System.Data.IDbConnection connection, System.Data.IDbTransaction transaction)
        {
            string sql = $"""
              SELECT TOP ({_outboxOptions.Batchsize}) id, content
              FROM outbox_messages WITH (UPDLOCK, ROWLOCK)
              WHERE processed_on_utc IS NULL
              ORDER BY occurred_on_utc
              """;
            var outboxMessages = await connection.QueryAsync<OutboxMessageResponse>(sql, transaction: transaction);
            return outboxMessages.ToList();
        }

        private async Task updateOutboxMessageAsync(System.Data.IDbConnection connection, System.Data.IDbTransaction transaction, OutboxMessageResponse outBoxMessage, Exception? exception)
        {
            string sql = $"""
              UPDATE outbox_messages
              SET processed_on_utc = @processedOnUtc,
                  error = @error
              WHERE id = @id
              """;
            await connection.ExecuteAsync(sql, new
            {
                processedOnUtc = _dateTimeProvider.UtcNow,
                error = exception?.ToString(),
                id = outBoxMessage.Id
            }, transaction: transaction);
        }

    }
}
