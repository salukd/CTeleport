namespace WeatherForecast.Infrastructure.Services.WeatherForecast.Mappers;

public static class OpenWeatherMapping
{
    public static OpenWeatherForecastResponse MapToWeatherForecastDto(this OpenWeatherForecastApiResponse result)
    {
        return MapResponse(result);
    }
    
    private static OpenWeatherForecastResponse MapResponse(OpenWeatherForecastApiResponse response)
    {
        return new OpenWeatherForecastResponse
        {
            CityName = response.City.Name,
            DailyForecasts = response.List.Select(MapForecast).ToList()
        };
    }

    private static DailyForecast MapForecast(List forecast)
    {
        return new DailyForecast
        {
            Date = DateTimeOffset.FromUnixTimeSeconds(forecast.Dt).DateTime,
            Temperature = forecast.Main.Temp,
            Humidity = forecast.Main.Humidity,
            WeatherDescriptions = forecast.Weather.Select(w => w.Description).ToList(),
            WindSpeed = forecast.Wind.Speed
        };
    }
}