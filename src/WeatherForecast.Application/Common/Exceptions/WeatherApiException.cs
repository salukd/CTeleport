namespace WeatherForecast.Application.Common.Exceptions;

public class WeatherApiException(string message, string errorCode, HttpStatusCode statusCode)
    : Exception(message)
{
    public string ErrorCode { get; } = errorCode;
    public HttpStatusCode StatusCode { get; } = statusCode;
}