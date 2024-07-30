namespace WeatherForecast.Application.Common.Services.WeatherForecast;

public interface IWeatherForecastService
{
    Task<string> GetWeatherForecastAsync(CancellationToken cancellationToken);
}