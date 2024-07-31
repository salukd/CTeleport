namespace WeatherForecast.Application.Common.Services.WeatherForecast;

public interface IWeatherForecastService
{
    Task<OpenWeatherForecastResponse> GetWeatherForecastAsync(string city, CancellationToken cancellationToken);
}