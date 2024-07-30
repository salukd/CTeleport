namespace WeatherForecast.Api.Rest.ExceptionHandlers;

public class CustomExceptionHandler : IExceptionHandler
{
    private readonly Dictionary<Type, Func<HttpContext, Exception, Task>> _exceptionHandlers;
    private readonly ILogger<CustomExceptionHandler> _logger;

    public CustomExceptionHandler(ILogger<CustomExceptionHandler> logger)
    {
        _logger = logger;
        _exceptionHandlers = new()
        {
            { typeof(ValidationException), HandleValidationExceptionAsync },
            { typeof(WeatherApiException), HandleWeatherApiExceptionAsync }
        };
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        var exceptionType = exception.GetType();

        if (_exceptionHandlers.TryGetValue(exceptionType, out var handler))
        {
            await handler(httpContext, exception);
            return true;
        }

        _logger.LogError(exception, "Unhandled exception occurred");
        await HandleUnknownExceptionAsync(httpContext, exception);
        return false;
    }

    private async Task HandleValidationExceptionAsync(HttpContext httpContext, Exception ex)
    {
        var exception = (ValidationException)ex;
        _logger.LogWarning("Validation error occurred: {@ValidationErrors}", exception.Errors);

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        await httpContext.Response.WriteAsJsonAsync(new ValidationProblemDetails(exception.Errors)
        {
            Status = StatusCodes.Status400BadRequest,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        });
    }
    private async Task HandleWeatherApiExceptionAsync(HttpContext httpContext, Exception ex)
    {
        var exception = (WeatherApiException)ex;
        _logger.LogError(ex, "Weather API error occurred: {ErrorCode}", exception.ErrorCode);

        httpContext.Response.StatusCode = (int)exception.StatusCode;
        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = (int)exception.StatusCode,
            Type =
                $"https://tools.ietf.org/html/rfc7231#section-6.{(int)exception.StatusCode / 100}.{(int)exception.StatusCode % 100}",
            Title = "Weather API Error",
            Detail = exception.Message,
            Extensions = { { "weatherApiErrorCode", exception.ErrorCode } }
        });
    }

    private async Task HandleUnknownExceptionAsync(HttpContext httpContext, Exception exception)
    {
        _logger.LogError(exception, "An unhandled exception has occurred");

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Title = "An error occurred while processing your request.",
            Detail = "An unexpected error occurred. Please try again later."
        });
    }
}