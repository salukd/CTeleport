namespace WeatherForecast.Infrastructure.ExternalApis.Policies;

public static class PolicyFactory<T> where T : IPolicyOptions
{
    public static IAsyncPolicy<HttpResponseMessage> GetTimeoutAndRetryPolicy(T options)
    {
        return Policy.WrapAsync(
            GetTimeoutPolicy(options),
            GetRetryPolicy(options)
        );
    }

    public static IAsyncPolicy<HttpResponseMessage> GetRateLimitPolicy(T options)
    {
        return Policy
            .HandleResult<HttpResponseMessage>(r => r.StatusCode == HttpStatusCode.TooManyRequests)
            .WaitAndRetryAsync(options.RetryPolicyOptions.RateLimiter,
                retryAttempt => TimeSpan.FromSeconds(retryAttempt));
    }
    
    public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(T options)
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: options.RetryPolicyOptions.CircuitBreakerAllowedBeforeBreaking,
                durationOfBreak: TimeSpan.FromSeconds(options.RetryPolicyOptions.CircuitBreakerBreakDuration)
            );
    }

    private static AsyncTimeoutPolicy<HttpResponseMessage> GetTimeoutPolicy(T options)
    {
        return Policy.TimeoutAsync<HttpResponseMessage>(
            TimeSpan.FromMilliseconds(options.RetryPolicyOptions.TimeoutInMilliseconds));
    }

    private static AsyncRetryPolicy<HttpResponseMessage> GetRetryPolicy(T options)
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutRejectedException>()
            .WaitAndRetryAsync(options.RetryPolicyOptions.RetryCount,
                _ => TimeSpan.FromMilliseconds(options.RetryPolicyOptions.RetryTimeInMilliseconds));
    }
}