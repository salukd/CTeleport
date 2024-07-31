using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using WeatherForecast.Application.WeatherForecast.Queries;

namespace WeatherForecast.Api.Rest.IntegrationTests.Controllers;

[Collection("ApplicationFactory")]
public class WeatherForecastControllerTests
{
    private readonly HttpClient _httpClient;

    public WeatherForecastControllerTests(ApplicationFactory applicationFactory)
    {
        _httpClient = applicationFactory.CreateClient();
    }

    [Fact]
    public async Task GetWeatherForecast_ShouldReturn_Data()
    {
        var date = DateTime.Now.Date.ToString("YYYY-MM-dddd");
        var response = await _httpClient.GetAsync($"api/v1.0/WeatherForecasts?city=Tbilisi&date={date}");


        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var weatherResponse = await response.Content.ReadFromJsonAsync<WeatherForecastResponse>();

        weatherResponse!.CityName.Should().Be("Tbilisi");
        weatherResponse!.DailyForecasts.Should().HaveCount(40);
    }
}