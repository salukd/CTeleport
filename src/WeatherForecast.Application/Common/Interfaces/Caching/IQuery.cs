namespace WeatherForecast.Application.Common.Interfaces.Caching;

public interface IQuery<out TResponse> : IRequest<TResponse>;