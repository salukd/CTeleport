using FluentAssertions;
using NSubstitute;
using WeatherForecast.Application.Common.Services.WeatherForecast;
using WeatherForecast.Application.Common.Services.WeatherForecast.Models;
using WeatherForecast.Application.WeatherForecast.Queries;

namespace WeatherForecast.Application.UnitTests.WeatherForecast;

public class GetWeatherForecastQueryHandlerTests
{
    [Fact]
    public void GetWeatherForecastQuery_CacheKey_ShouldBeCorrectlyFormatted()
    {
        // Arrange
        var city = "London";
        var date = new DateTime(2023, 7, 1);
        var query = new GetWeatherForecastQuery(city, date);

        // Act
        var cacheKey = query.CacheKey;

        // Assert
        cacheKey.Should().Be("get-weather-forecast-open-weather-query_London_2023-07-01");
    }

    [Fact]
    public void GetWeatherForecastQuery_CacheKey_ShouldHandleNullDate()
    {
        // Arrange
        var city = "New York";
        var query = new GetWeatherForecastQuery(city, null);

        // Act
        var cacheKey = query.CacheKey;

        // Assert
        cacheKey.Should().Be("get-weather-forecast-open-weather-query_New York_all");
    }

    [Fact]
    public void GetWeatherForecastQuery_Expiration_ShouldBe300Seconds()
    {
        // Arrange
        var query = new GetWeatherForecastQuery("Berlin", null);

        // Act & Assert
        query.Expiration.Should().Be(TimeSpan.FromSeconds(300));
    }

    [Fact]
    public async Task GetWeatherForecastQueryHandler_Handle_ShouldReturnCorrectResponse()
    {
        // Arrange
        var weatherForecastService = Substitute.For<IWeatherForecastService>();
        var handler = new GetWeatherForecastQueryHandler(weatherForecastService);
        var query = new GetWeatherForecastQuery("Paris", new DateTime(2023, 7, 1));

        var openWeatherResponse = new OpenWeatherForecastResponse
        {
            CityName = "Paris",
            DailyForecasts = new List<DailyForecast>
            {
                new DailyForecast { Date = new DateTime(2023, 7, 1), Temperature = 25 },
                new DailyForecast { Date = new DateTime(2023, 7, 2), Temperature = 26 }
            }
        };

        weatherForecastService.GetWeatherForecastAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(openWeatherResponse);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.CityName.Should().Be("Paris");
        result.DailyForecasts.Should().HaveCount(1);
        result.DailyForecasts[0].Date.Should().Be(new DateTime(2023, 7, 1));
    }

    [Fact]
    public async Task GetWeatherForecastQueryHandler_Handle_ShouldReturnAllForecastsWhenDateIsNull()
    {
        // Arrange
        var weatherForecastService = Substitute.For<IWeatherForecastService>();
        var handler = new GetWeatherForecastQueryHandler(weatherForecastService);
        var query = new GetWeatherForecastQuery("Tokyo", null);

        var openWeatherResponse = new OpenWeatherForecastResponse
        {
            CityName = "Tokyo",
            DailyForecasts = new List<DailyForecast>
            {
                new DailyForecast { Date = new DateTime(2023, 7, 1), Temperature = 30 },
                new DailyForecast { Date = new DateTime(2023, 7, 2), Temperature = 31 }
            }
        };

        weatherForecastService.GetWeatherForecastAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(openWeatherResponse);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.CityName.Should().Be("Tokyo");
        result.DailyForecasts.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetWeatherForecastQueryHandler_Handle_ShouldHandleEmptyForecastList()
    {
        // Arrange
        var weatherForecastService = Substitute.For<IWeatherForecastService>();
        var handler = new GetWeatherForecastQueryHandler(weatherForecastService);
        var query = new GetWeatherForecastQuery("Empty City", new DateTime(2023, 7, 1));

        var openWeatherResponse = new OpenWeatherForecastResponse
        {
            CityName = "Empty City",
            DailyForecasts = new List<DailyForecast>()
        };

        weatherForecastService.GetWeatherForecastAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(openWeatherResponse);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.CityName.Should().Be("Empty City");
        result.DailyForecasts.Should().BeEmpty();
    }

    [Fact]
    public async Task GetWeatherForecastQueryHandler_Handle_ShouldHandleDateOutsideAvailableForecasts()
    {
        // Arrange
        var weatherForecastService = Substitute.For<IWeatherForecastService>();
        var handler = new GetWeatherForecastQueryHandler(weatherForecastService);
        var query = new GetWeatherForecastQuery("Rome", new DateTime(2023, 8, 1)); // Date not in forecast

        var openWeatherResponse = new OpenWeatherForecastResponse
        {
            CityName = "Rome",
            DailyForecasts = new List<DailyForecast>
            {
                new DailyForecast { Date = new DateTime(2023, 7, 1), Temperature = 28 },
                new DailyForecast { Date = new DateTime(2023, 7, 2), Temperature = 29 }
            }
        };

        weatherForecastService.GetWeatherForecastAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(openWeatherResponse);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.CityName.Should().Be("Rome");
        result.DailyForecasts.Should().BeEmpty();
    }
}