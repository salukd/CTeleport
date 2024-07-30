namespace WeatherForecast.Infrastructure.ExternalApis.OpenWeather;

public interface IOpenWeatherApi
{
    [Get("/data/2.5/forecast")]
    Task<OpenWeatherForecastApiResponse> GetFiveDayWeatherForecastByCityAsync(
        [Query] string q,
        [Query] string units = "metric",
        CancellationToken cancellationToken = default);
}