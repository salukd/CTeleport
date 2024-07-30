namespace WeatherForecast.Application.WeatherForecast.Queries;

public record GetWeatherForecastQuery(string City, DateTime? Date) : ICachedQuery<WeatherForecastResponse>
{
    public string CacheKey => $"get-weather-forecast-open-weather-query_{City}_{Date?.ToString("yyyy-MM-dd") ?? "all"}";
    public TimeSpan? Expiration => TimeSpan.FromSeconds(300);
}

public class GetWeatherForecastQueryHandler(IWeatherForecastService weatherForecastService)
    : IRequestHandler<GetWeatherForecastQuery, WeatherForecastResponse>
{
    public async Task<WeatherForecastResponse> Handle(GetWeatherForecastQuery request,
        CancellationToken cancellationToken)
    {
        var openWeatherResult = await weatherForecastService.GetWeatherForecastAsync(request.City, cancellationToken);

        return ConvertToWeatherForecastResponse(openWeatherResult, request.Date);
    }

    private WeatherForecastResponse ConvertToWeatherForecastResponse(OpenWeatherForecastResponse openWeatherResult,
        DateTime? date)
    {
        var dailyForecasts = FilterForecastsByDate(openWeatherResult.DailyForecasts, date);

        return new WeatherForecastResponse
        {
            CityName = openWeatherResult.CityName,
            DailyForecasts = dailyForecasts.ConvertAll(forecast => forecast)
        };
    }

    private List<DailyForecast> FilterForecastsByDate(List<DailyForecast> forecasts, DateTime? date)
    {
        if (!date.HasValue)
        {
            return forecasts;
        }

        var targetDate = date.Value.Date;
        return forecasts
            .Where(forecast => forecast.Date.Date == targetDate)
            .ToList();
    }
}