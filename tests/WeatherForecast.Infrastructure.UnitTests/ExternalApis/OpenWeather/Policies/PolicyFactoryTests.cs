using System.Net;
using FluentAssertions;
using NSubstitute;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Wrap;
using WeatherForecast.Infrastructure.ExternalApis.Policies;

namespace WeatherForecast.Infrastructure.UnitTests.ExternalApis.OpenWeather.Policies;

public class PolicyFactoryTests
{
    private readonly IPolicyOptions _mockOptions;
    private readonly RetryPolicyOptions _retryPolicyOptions;

    public PolicyFactoryTests()
    {
        _mockOptions = Substitute.For<IPolicyOptions>();
        _retryPolicyOptions = new RetryPolicyOptions
        {
            TimeoutInMilliseconds = 5000,
            RetryCount = 3,
            RetryTimeInMilliseconds = 1000,
            RateLimiter = 5,
            CircuitBreakerAllowedBeforeBreaking = 5,
            CircuitBreakerBreakDuration = 30
        };
        _mockOptions.RetryPolicyOptions.Returns(_retryPolicyOptions);
    }

    [Fact]
    public void GetTimeoutAndRetryPolicy_ShouldReturnCombinedPolicy()
    {
        // Act
        var policy = PolicyFactory<IPolicyOptions>.GetTimeoutAndRetryPolicy(_mockOptions);

        // Assert
        policy.Should().NotBeNull();
        policy.Should().BeOfType<AsyncPolicyWrap<HttpResponseMessage>>();
    }

    [Fact]
    public void GetRateLimitPolicy_ShouldReturnCorrectPolicy()
    {
        // Act
        var policy = PolicyFactory<IPolicyOptions>.GetRateLimitPolicy(_mockOptions);

        // Assert
        policy.Should().NotBeNull();
        policy.Should().BeOfType<AsyncRetryPolicy<HttpResponseMessage>>();
    }

    [Fact]
    public void GetCircuitBreakerPolicy_ShouldReturnCorrectPolicy()
    {
        // Act
        var policy = PolicyFactory<IPolicyOptions>.GetCircuitBreakerPolicy(_mockOptions);

        // Assert
        policy.Should().NotBeNull();
        policy.Should().BeOfType<AsyncCircuitBreakerPolicy<HttpResponseMessage>>();
    }
    

    [Fact]
    public async Task GetRateLimitPolicy_ShouldRetryOnTooManyRequests()
    {
        // Arrange
        var policy = PolicyFactory<IPolicyOptions>.GetRateLimitPolicy(_mockOptions);
        int executionCount = 0;

        // Act
        await policy.ExecuteAsync(() =>
        {
            executionCount++;
            return Task.FromResult(executionCount < 3
                ? new HttpResponseMessage(HttpStatusCode.TooManyRequests)
                : new HttpResponseMessage(HttpStatusCode.OK));
        });

        // Assert
        executionCount.Should().Be(3);
    }

    [Fact]
    public async Task GetCircuitBreakerPolicy_ShouldBreakCircuitAfterSpecifiedFailures()
    {
        // Arrange
        var policy = PolicyFactory<IPolicyOptions>.GetCircuitBreakerPolicy(_mockOptions);
        int executionCount = 0;

        // Act & Assert
        for (int i = 0; i < _retryPolicyOptions.CircuitBreakerAllowedBeforeBreaking + 1; i++)
        {
            try
            {
                await policy.ExecuteAsync(() =>
                {
                    executionCount++;
                    throw new HttpRequestException();
                });
            }
            catch (HttpRequestException)
            {
                // Expected for the first few calls
            }
            catch (BrokenCircuitException)
            {
                // Expected after circuit breaks
                break;
            }
        }

        executionCount.Should().Be(_retryPolicyOptions.CircuitBreakerAllowedBeforeBreaking);
    }
}