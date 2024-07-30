using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System.Net;
using Polly.Timeout;
using RestEase;
using WeatherForecast.Application.Common.Exceptions;
using WeatherForecast.Infrastructure.ExternalApis.OpenWeather;
using WeatherForecast.Infrastructure.ExternalApis.OpenWeather.Models;
using WeatherForecast.Infrastructure.Services.WeatherForecast;

public class WeatherForecastServiceTests
{
    private readonly IOpenWeatherApi _mockOpenWeatherApi;
    private readonly WeatherForecastService _service;

    public WeatherForecastServiceTests()
    {
        _mockOpenWeatherApi = Substitute.For<IOpenWeatherApi>();
        _service = new WeatherForecastService(_mockOpenWeatherApi);
    }

    private ApiException CreateApiException(HttpStatusCode statusCode, string reasonPhrase, string content)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "http://example.com/weather");
        var response = new HttpResponseMessage(statusCode)
        {
            ReasonPhrase = reasonPhrase,
            Content = new StringContent(content)
        };

        return new ApiException(request, response, content);
    }

    [Fact]
    public async Task GetWeatherForecastAsync_SuccessfulResponse_ShouldReturnMappedForecast()
    {
        // Arrange
        var city = "TestCity";
        var apiResponse = new OpenWeatherForecastApiResponse
        {
            Cod = "200",
            Message = 0,
            Cnt = 40,
            List = new List<List>
            {
                new List
                {
                    Dt = 1623456789,
                    Main = new Main
                    {
                        Temp = 20.5,
                        FeelsLike = 21.2,
                        TempMin = 19.8,
                        TempMax = 21.5,
                        Pressure = 1015,
                        SeaLevel = 1015,
                        GrndLevel = 1010,
                        Humidity = 65,
                        TempKf = 0.2
                    },
                    Weather = new List<Weather>
                    {
                        new Weather
                        {
                            Id = 800,
                            Main = "Clear",
                            Description = "clear sky",
                            Icon = "01d"
                        }
                    },
                    Clouds = new Clouds { All = 0 },
                    Wind = new Wind { Speed = 3.5, Deg = 120, Gust = 5.2 },
                    Visibility = 10000,
                    Pop = 0.1,
                    Sys = new Sys { Pod = "d" },
                    DtTxt = "2023-06-12 12:00:00"
                }
            },
            City = new City
            {
                Id = 12345,
                Name = city,
                Coord = new Coord { Lat = 51.5074, Lon = -0.1278 },
                Country = "GB",
                Population = 8982000,
                Timezone = 3600,
                Sunrise = 1623470400,
                Sunset = 1623528000
            }
        };

        _mockOpenWeatherApi.GetFiveDayWeatherForecastByCityAsync(city, cancellationToken: Arg.Any<CancellationToken>())
            .Returns(apiResponse);

        // Act
        var result = await _service.GetWeatherForecastAsync(city, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.CityName.Should().Be(city);
        result.DailyForecasts.Should().HaveCount(1);
        var forecast = result.DailyForecasts[0];
        forecast.Date.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1623456789).DateTime);
        forecast.Temperature.Should().Be(20.5);
        forecast.Humidity.Should().Be(65);
        forecast.WindSpeed.Should().Be(3.5);
    }

    [Fact]
    public async Task GetWeatherForecastAsync_CityNotFound_ShouldThrowWeatherApiException()
    {
        // Arrange
        var city = "NonExistentCity";
        var apiException = CreateApiException(HttpStatusCode.NotFound, "Not Found", "City not found");

        _mockOpenWeatherApi.GetFiveDayWeatherForecastByCityAsync(city, cancellationToken: Arg.Any<CancellationToken>())
            .Throws(apiException);

        // Act
        Func<Task> act = async () => await _service.GetWeatherForecastAsync(city, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<WeatherApiException>()
            .Where(ex => ex.Message == "The specified city was not found."
                         && ex.ErrorCode == "CITY_NOT_FOUND"
                         && ex.StatusCode == HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetWeatherForecastAsync_InvalidApiKey_ShouldThrowWeatherApiException()
    {
        // Arrange
        var city = "TestCity";
        var apiException = CreateApiException(HttpStatusCode.Unauthorized, "Unauthorized", "Invalid API key");

        _mockOpenWeatherApi.GetFiveDayWeatherForecastByCityAsync(city, cancellationToken: Arg.Any<CancellationToken>())
            .Throws(apiException);

        // Act
        Func<Task> act = async () => await _service.GetWeatherForecastAsync(city, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<WeatherApiException>()
            .Where(ex => ex.Message == "Invalid API key."
                         && ex.ErrorCode == "INVALID_API_KEY"
                         && ex.StatusCode == HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetWeatherForecastAsync_RateLimitExceeded_ShouldThrowWeatherApiException()
    {
        // Arrange
        var city = "TestCity";
        var apiException = CreateApiException(HttpStatusCode.TooManyRequests, "Too Many Requests", "Rate limit exceeded");

        _mockOpenWeatherApi.GetFiveDayWeatherForecastByCityAsync(city, cancellationToken: Arg.Any<CancellationToken>())
            .Throws(apiException);

        // Act
        Func<Task> act = async () => await _service.GetWeatherForecastAsync(city, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<WeatherApiException>()
            .Where(ex => ex.Message == "Rate limit exceeded. Please try again later."
                         && ex.ErrorCode == "RATE_LIMIT_EXCEEDED"
                         && ex.StatusCode == HttpStatusCode.TooManyRequests);
    }

    [Fact]
    public async Task GetWeatherForecastAsync_BadRequest_ShouldThrowWeatherApiException()
    {
        // Arrange
        var city = "TestCity";
        var apiException = CreateApiException(HttpStatusCode.BadRequest, "Bad Request", "Invalid request");

        _mockOpenWeatherApi.GetFiveDayWeatherForecastByCityAsync(city, cancellationToken: Arg.Any<CancellationToken>())
            .Throws(apiException);

        // Act
        Func<Task> act = async () => await _service.GetWeatherForecastAsync(city, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<WeatherApiException>()
            .Where(ex => ex.Message == "An unexpected error occurred with the weather service."
                         && ex.ErrorCode == "WEATHER_API_ERROR"
                         && ex.StatusCode == HttpStatusCode.BadGateway);
    }

    [Fact]
    public async Task GetWeatherForecastAsync_TimeoutException_ShouldThrowWeatherApiException()
    {
        // Arrange
        var city = "TestCity";
        _mockOpenWeatherApi.GetFiveDayWeatherForecastByCityAsync(city, cancellationToken: Arg.Any<CancellationToken>())
            .Throws(new TimeoutRejectedException());

        // Act
        Func<Task> act = async () => await _service.GetWeatherForecastAsync(city, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<WeatherApiException>()
            .Where(ex => ex.Message == "The weather service is currently unavailable. Please try again later."
                         && ex.ErrorCode == "SERVICE_TIMEOUT"
                         && ex.StatusCode == HttpStatusCode.GatewayTimeout);
    }

    [Fact]
    public async Task GetWeatherForecastAsync_UnexpectedException_ShouldThrowWeatherApiException()
    {
        // Arrange
        var city = "TestCity";
        _mockOpenWeatherApi.GetFiveDayWeatherForecastByCityAsync(city, cancellationToken: Arg.Any<CancellationToken>())
            .Throws(new Exception("Unexpected error"));

        // Act
        Func<Task> act = async () => await _service.GetWeatherForecastAsync(city, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<WeatherApiException>()
            .Where(ex => ex.Message == "An unexpected error occurred. Please try again later."
                         && ex.ErrorCode == "UNKNOWN_ERROR"
                         && ex.StatusCode == HttpStatusCode.InternalServerError);
    }
}