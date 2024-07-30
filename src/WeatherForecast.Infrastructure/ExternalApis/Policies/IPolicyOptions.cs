namespace WeatherForecast.Infrastructure.ExternalApis.Policies;

public interface IPolicyOptions
{
    RetryPolicyOptions RetryPolicyOptions { get; set; }
}

public class RetryPolicyOptions
{
    public int TimeoutInMilliseconds { get; set; }
    public int RetryCount { get; set; }
    public int RetryTimeInMilliseconds { get; set; }
    public int RateLimiter { get; set; }
    public int CircuitBreakerAllowedBeforeBreaking { get; set; } 
    public int CircuitBreakerBreakDuration { get; set; } 
}