namespace WeatherForecast.Api.Rest.Controllers;

[ApiVersion("1.0")]
public class WeatherForecastsController(ISender mediator) : ApiController(mediator)
{
    [HttpGet]
    [SwaggerOperation(Summary = "Get weather forecast",
        Description = "Retrieves the weather forecast for a specified city.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Successfully retrieved the weather forecast",
        typeof(WeatherForecastResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest,
        "Invalid input - This could be due to an invalid city name or date format")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Weather forecast not found for the specified city or date")]
    [SwaggerResponse(StatusCodes.Status429TooManyRequests, "Too many requests - API rate limit exceeded")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error occurred")]
    [SwaggerResponse(StatusCodes.Status502BadGateway,
        "Bad gateway - Error occurred while fetching data from external weather service")]
    [SwaggerResponse(StatusCodes.Status503ServiceUnavailable,
        "Service unavailable - The weather service is temporarily unavailable")]
    [SwaggerResponse(StatusCodes.Status504GatewayTimeout,
        "Gateway timeout - The weather service took too long to respond")]
    public async Task<IActionResult> GetForecast(
        [FromQuery] [SwaggerParameter("The name of the city", Required = true)] string city,
        [FromQuery] [SwaggerParameter("The date for the forecast (YYYY-MM-DD). If not provided, returns +5 days forecast.")]
        DateTime? date,
        CancellationToken cancellationToken)
    {
        var openWeatherResult =
            await Mediator.Send(new GetWeatherForecastQuery(city, date), cancellationToken);

        return Ok(openWeatherResult);
    }
}