namespace WeatherForecast.Application.Common.Behaviours;

public sealed class QueryCachingBehaviour<TRequest, TResponse>(ICacheService cacheService)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICachedQuery
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        return await cacheService.GetOrCreateAsync(request.CacheKey,
            _ => next(),
            request.Expiration,
            cancellationToken);
    }
}