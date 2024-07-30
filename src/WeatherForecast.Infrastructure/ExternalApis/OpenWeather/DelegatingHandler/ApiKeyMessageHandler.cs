namespace WeatherForecast.Infrastructure.ExternalApis.OpenWeather.DelegatingHandler;

public class ApiKeyMessageHandler(string apiKey) : System.Net.Http.DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var uriBuilder = new UriBuilder(request.RequestUri!);
        var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
        query["appid"] = apiKey;
        uriBuilder.Query = query.ToString();
        request.RequestUri = uriBuilder.Uri;

        return base.SendAsync(request, cancellationToken);
    }
}