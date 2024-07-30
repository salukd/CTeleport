using System.Net;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using RestEase;
using StackExchange.Redis;
using WeatherForecast.Application.Common.Services.WeatherForecast.Models;
using WeatherForecast.Application.WeatherForecast.Queries;
using WeatherForecast.Infrastructure.ExternalApis.OpenWeather;
using WeatherForecast.Infrastructure.ExternalApis.OpenWeather.Models;

namespace WeatherForecast.Api.Rest.IntegrationTests.Controllers;

public class WeatherForecastApiIntegrationTests : IClassFixture<CustomWebApplicationFactory<IApiMarker>>, IDisposable
{
    private readonly HttpClient _client;
    private readonly IConnectionMultiplexer _redis;
    private readonly IOpenWeatherApi _mockOpenWeatherApi;

    public WeatherForecastApiIntegrationTests(CustomWebApplicationFactory<IApiMarker> factory)
    {
        CustomWebApplicationFactory<IApiMarker> factory1 = factory;
        _client = factory1.CreateClient();
        _redis = factory1.Services.GetRequiredService<IConnectionMultiplexer>();
        _mockOpenWeatherApi = factory1.Services.GetRequiredService<IOpenWeatherApi>();
    }

    [Fact]
    public async Task GetForecast_ValidCity_ReturnsOkWithForecast()
    {
        // Arrange
        var city = "London";
        var date = DateTime.UtcNow.Date;
        var cacheKey = $"get-weather-forecast-open-weather-query_{city}_{date:yyyy-MM-dd}";

        var mockResponse = new OpenWeatherForecastApiResponse
        {
            City = new City { Name = city },
            List = new List<List>
            {
                new List
                {
                    Dt = ((DateTimeOffset)date).ToUnixTimeSeconds(),
                    Main = new Main { Temp = 20, FeelsLike = 22 },
                    Weather = new List<Weather> { new() { Description = "Clear sky" } }
                }
            }
        };

        _mockOpenWeatherApi.GetFiveDayWeatherForecastByCityAsync(city, cancellationToken: Arg.Any<CancellationToken>())
            .Returns(mockResponse);

        // Act
        var response = await _client.GetAsync($"/api/v1/WeatherForecast?city={city}&date={date:yyyy-MM-dd}");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var forecast = JsonConvert.DeserializeObject<WeatherForecastResponse>(content);

        forecast.Should().NotBeNull();
        forecast.CityName.Should().Be(city);
        forecast.DailyForecasts.Should().HaveCount(1);
        forecast.DailyForecasts[0].Temperature.Should().Be(20);

        // Check if the result is cached in Redis
        var cache = _redis.GetDatabase();
        var cachedValue = await cache.StringGetAsync(cacheKey);
        cachedValue.Should().NotBeNull();
    }

    [Fact]
    public async Task GetForecast_CachedResult_ReturnsCachedForecast()
    {
        // Arrange
        var city = "Paris";
        var date = DateTime.UtcNow.Date;
        var cacheKey = $"get-weather-forecast-open-weather-query_{city}_{date:yyyy-MM-dd}";

        var cachedForecast = new WeatherForecastResponse
        {
            CityName = city,
            DailyForecasts = new List<DailyForecast>
            {
                new DailyForecast
                {
                    Date = date,
                    Temperature = 25,
                }
            }
        };

        var cache = _redis.GetDatabase();
        await cache.StringSetAsync(cacheKey, JsonConvert.SerializeObject(cachedForecast), TimeSpan.FromMinutes(5));

        // Act
        var response = await _client.GetAsync($"/api/v1/WeatherForecast?city={city}&date={date:yyyy-MM-dd}");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var forecast = JsonConvert.DeserializeObject<WeatherForecastResponse>(content);

        forecast.Should().BeEquivalentTo(cachedForecast);
        await _mockOpenWeatherApi.DidNotReceive().GetFiveDayWeatherForecastByCityAsync(Arg.Any<string>(), cancellationToken: Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetForecast_InvalidCity_ReturnsBadRequest()
    {
        // Arrange
        var city = "";

        // Act
        var response = await _client.GetAsync($"/api/v1/WeatherForecast?city={city}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetForecast_CityNotFound_ReturnsNotFound()
    {
        // Arrange
        var city = "NonExistentCity";
        _mockOpenWeatherApi.GetFiveDayWeatherForecastByCityAsync(city, cancellationToken: Arg.Any<CancellationToken>())
            .Throws(new ApiException(new HttpRequestMessage(), new HttpResponseMessage(HttpStatusCode.NotFound), "City not found"));

        // Act
        var response = await _client.GetAsync($"/api/v1/WeatherForecast?city={city}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    public void Dispose()
    {
        _redis.Dispose();
    }
}