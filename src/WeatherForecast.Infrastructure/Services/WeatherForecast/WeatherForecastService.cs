namespace WeatherForecast.Infrastructure.Services.WeatherForecast;

public class WeatherForecastService(IOpenWeatherApi openWeatherApi) : IWeatherForecastService
{
    private readonly IOpenWeatherApi _openWeatherApi = openWeatherApi ?? throw new ArgumentNullException(nameof(openWeatherApi));

    public async Task<OpenWeatherForecastResponse> GetWeatherForecastAsync(string city, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _openWeatherApi.GetFiveDayWeatherForecastByCityAsync(city, 
                cancellationToken: cancellationToken);
            
            return response.MapToWeatherForecastDto();
        }
        catch (ApiException apiEx)
        {
            throw MapApiException(apiEx);
        }
        catch (TimeoutRejectedException)
        {
            throw new WeatherApiException("The weather service is currently unavailable. Please try again later.",
                "SERVICE_TIMEOUT", HttpStatusCode.GatewayTimeout);
        }
        catch (Exception)
        {
            throw new WeatherApiException("An unexpected error occurred. Please try again later.", "UNKNOWN_ERROR",
                HttpStatusCode.InternalServerError);
        }
    }

    private WeatherApiException MapApiException(ApiException apiEx)
    {
        return apiEx.StatusCode switch
        {
            HttpStatusCode.NotFound => new WeatherApiException("The specified city was not found.", "CITY_NOT_FOUND",
                HttpStatusCode.NotFound),
            HttpStatusCode.Unauthorized => new WeatherApiException("Invalid API key.", "INVALID_API_KEY",
                HttpStatusCode.Unauthorized),
            HttpStatusCode.TooManyRequests => new WeatherApiException("Rate limit exceeded. Please try again later.",
                "RATE_LIMIT_EXCEEDED", HttpStatusCode.TooManyRequests),
            _ => new WeatherApiException("An unexpected error occurred with the weather service.", "WEATHER_API_ERROR",
                HttpStatusCode.BadGateway)
        };
    }
}