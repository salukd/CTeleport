namespace WeatherForecast.Api.Rest.Health;

public class OpenWeatherMapHealthCheck(IOpenWeatherApi openWeatherApi) : IHealthCheck
{
    private readonly IOpenWeatherApi _openWeatherApi = openWeatherApi ?? 
                                                       throw new ArgumentNullException(nameof(openWeatherApi));

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _openWeatherApi.GetFiveDayWeatherForecastByCityAsync("Tbilisi", cancellationToken: cancellationToken);
            return HealthCheckResult.Healthy("OpenWeatherMap API is accessible.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Failed to connect to OpenWeatherMap API.", ex);
        }
    }
}