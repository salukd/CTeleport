namespace WeatherForecast.Application.Common.Services.WeatherForecast.Models;

public class OpenWeatherForecastResponse
{
    public string CityName { get; set; } = null!;
    public List<DailyForecast> DailyForecasts { get; set; } = null!;
}

public class DailyForecast
{
    public DateTime Date { get; set; }
    public double Temperature { get; set; }
    public int Humidity { get; set; }
    public List<string> WeatherDescriptions { get; set; } = null!;
    public double WindSpeed { get; set; }
}