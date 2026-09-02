using bookify.domain.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Bookify.Application.Abstraction.Behavior
{
    public class LoggingBeahvior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IBaseRequest where TResponse : Result
    {
        private readonly ILogger<LoggingBeahvior<TRequest, TResponse>> _logger;

        public LoggingBeahvior(ILogger<LoggingBeahvior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = request.GetType().Name;
            try
            {
                _logger.LogInformation($"Executing request {requestName}");
                var result = await next();
                if (result.IsSuccess)
                {
                    _logger.LogInformation($"Executed request {requestName} succeeded");

                }
                else
                {
                    using (LogContext.PushProperty("Errors", result.Error, true))
                        _logger.LogError($"Executed request {requestName} failed with errors");
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Executed request {requestName} failed");
                throw;
            }
        }
    }
}
