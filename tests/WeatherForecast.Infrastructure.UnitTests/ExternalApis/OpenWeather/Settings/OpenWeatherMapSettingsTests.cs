using FluentAssertions;
using WeatherForecast.Infrastructure.ExternalApis.OpenWeather.Settings;
using WeatherForecast.Infrastructure.ExternalApis.Policies;

namespace WeatherForecast.Infrastructure.UnitTests.ExternalApis.OpenWeather.Settings;

public class OpenWeatherMapSettingsTests
{
    [Fact]
    public void OpenWeatherMapSettings_ShouldImplementIPolicyOptions()
    {
        // Arrange & Act
        var settings = new OpenWeatherMapSettings();

        // Assert
        settings.Should().BeAssignableTo<IPolicyOptions>();
    }

    [Fact]
    public void Address_ShouldBeSettableAndGettable()
    {
        // Arrange
        var settings = new OpenWeatherMapSettings();
        var testAddress = "http://api.openweathermap.org/data/2.5";

        // Act
        settings.Address = testAddress;

        // Assert
        settings.Address.Should().Be(testAddress);
    }

    [Fact]
    public void ApiKey_ShouldBeSettableAndGettable()
    {
        // Arrange
        var settings = new OpenWeatherMapSettings();
        var testApiKey = "testApiKey123";

        // Act
        settings.ApiKey = testApiKey;

        // Assert
        settings.ApiKey.Should().Be(testApiKey);
    }

    [Fact]
    public void RetryPolicyOptions_ShouldBeSettableAndGettable()
    {
        // Arrange
        var settings = new OpenWeatherMapSettings();
        var testRetryPolicyOptions = new RetryPolicyOptions
        {
            TimeoutInMilliseconds = 5000,
            RetryCount = 3,
            RetryTimeInMilliseconds = 1000,
            RateLimiter = 10,
            CircuitBreakerAllowedBeforeBreaking = 5,
            CircuitBreakerBreakDuration = 30000
        };

        // Act
        settings.RetryPolicyOptions = testRetryPolicyOptions;

        // Assert
        settings.RetryPolicyOptions.Should().BeEquivalentTo(testRetryPolicyOptions);
    }
    

    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange & Act
        var settings = new OpenWeatherMapSettings
        {
            Address = "http://api.example.com",
            ApiKey = "exampleApiKey",
            RetryPolicyOptions = new RetryPolicyOptions
            {
                TimeoutInMilliseconds = 5000,
                RetryCount = 3,
                RetryTimeInMilliseconds = 1000,
                RateLimiter = 10,
                CircuitBreakerAllowedBeforeBreaking = 5,
                CircuitBreakerBreakDuration = 30000
            }
        };

        // Assert
        settings.Address.Should().Be("http://api.example.com");
        settings.ApiKey.Should().Be("exampleApiKey");
        settings.RetryPolicyOptions.Should().NotBeNull();
        settings.RetryPolicyOptions.TimeoutInMilliseconds.Should().Be(5000);
        settings.RetryPolicyOptions.RetryCount.Should().Be(3);
        settings.RetryPolicyOptions.RetryTimeInMilliseconds.Should().Be(1000);
        settings.RetryPolicyOptions.RateLimiter.Should().Be(10);
        settings.RetryPolicyOptions.CircuitBreakerAllowedBeforeBreaking.Should().Be(5);
        settings.RetryPolicyOptions.CircuitBreakerBreakDuration.Should().Be(30000);
    }
}

public class RetryPolicyOptionsTests
{
    [Fact]
    public void RetryPolicyOptions_PropertiesShouldBeSettableAndGettable()
    {
        // Arrange
        var options = new RetryPolicyOptions();

        // Act
        options.TimeoutInMilliseconds = 5000;
        options.RetryCount = 3;
        options.RetryTimeInMilliseconds = 1000;
        options.RateLimiter = 10;
        options.CircuitBreakerAllowedBeforeBreaking = 5;
        options.CircuitBreakerBreakDuration = 30000;

        // Assert
        options.TimeoutInMilliseconds.Should().Be(5000);
        options.RetryCount.Should().Be(3);
        options.RetryTimeInMilliseconds.Should().Be(1000);
        options.RateLimiter.Should().Be(10);
        options.CircuitBreakerAllowedBeforeBreaking.Should().Be(5);
        options.CircuitBreakerBreakDuration.Should().Be(30000);
    }

    [Fact]
    public void RetryPolicyOptions_DefaultValuesShouldBeZero()
    {
        // Arrange & Act
        var options = new RetryPolicyOptions();

        // Assert
        options.TimeoutInMilliseconds.Should().Be(0);
        options.RetryCount.Should().Be(0);
        options.RetryTimeInMilliseconds.Should().Be(0);
        options.RateLimiter.Should().Be(0);
        options.CircuitBreakerAllowedBeforeBreaking.Should().Be(0);
        options.CircuitBreakerBreakDuration.Should().Be(0);
    }
}