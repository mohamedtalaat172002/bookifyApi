using Bookify.Application.Abstraction.Messaging;

namespace Bookify.Application.Abstraction.Caching
{
    public interface ICachedQuery<TResponse> : IQuery<TResponse>, ICachedQuery
    {
    }
    public interface ICachedQuery
    {
        string cacheKey { get; }
        TimeSpan? cacheExpiration { get; }
    }
}
