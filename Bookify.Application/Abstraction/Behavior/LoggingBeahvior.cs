using Bookify.Application.Abstraction.Messaging;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookify.Application.Abstraction.Behavior
{
    public class LoggingBeahvior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IBaseCommand
    {
        private readonly ILogger<TRequest> _logger;

        public LoggingBeahvior(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = request.GetType().Name;
            try
            {
                _logger.LogInformation($"Executing command {requestName}");
                var result = await next();
                _logger.LogInformation($"Executed command {requestName} succedded");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Executed command {requestName} failed");
                throw;
            }
        }
    }
}
