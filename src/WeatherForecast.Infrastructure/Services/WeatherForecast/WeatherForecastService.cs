using WeatherForecast.Application.Common.Services.WeatherForecast;

namespace WeatherForecast.Infrastructure.Services.WeatherForecast;

public class WeatherForecastService : IWeatherForecastService
{
    public async Task<string> GetWeatherForecastAsync(CancellationToken cancellationToken)
    {
        return string.Empty;
    }
}