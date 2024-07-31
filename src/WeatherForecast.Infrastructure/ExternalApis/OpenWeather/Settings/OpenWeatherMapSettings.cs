namespace WeatherForecast.Infrastructure.ExternalApis.OpenWeather.Settings;

public class OpenWeatherMapSettings : IPolicyOptions
{
    public string Address { get; set; } = null!;
    public string ApiKey { get; set; } = null!;
    public RetryPolicyOptions RetryPolicyOptions { get; set; } = null!;
}