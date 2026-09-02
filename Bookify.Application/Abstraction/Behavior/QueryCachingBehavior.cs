using bookify.domain.Abstractions;
using Bookify.Application.Abstraction.Caching;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookify.Application.Abstraction.Behavior
{
    public class QueryCachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICachedQuery
        where TResponse : Result
    {
        private readonly ICachService _cacheService;
        private readonly ILogger<QueryCachingBehavior<TRequest, TResponse>> _logger;

        public QueryCachingBehavior(ICachService cachService, ILogger<QueryCachingBehavior<TRequest, TResponse>> logger)
        {
            _cacheService = cachService;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            TResponse? cachedResult = await _cacheService.GetAsync<TResponse>(
           request.cacheKey,
           cancellationToken);

            string name = typeof(TRequest).Name;
            if (cachedResult is not null)
            {
                _logger.LogInformation("Cache hit for {Query}", name);

                return cachedResult;
            }

            _logger.LogInformation("Cache miss for {Query}", name);

            TResponse result = await next();

            if (result.IsSuccess)
            {
                await _cacheService.SetAsync(request.cacheKey, result, request.cacheExpiration, cancellationToken);
            }

            return result;
        }
    }
}
