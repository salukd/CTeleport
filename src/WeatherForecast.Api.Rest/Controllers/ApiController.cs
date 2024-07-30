namespace WeatherForecast.Api.Rest.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class ApiController : ControllerBase
{
    protected readonly ISender Mediator;

    public ApiController(ISender mediator)
    {
        Mediator = mediator;
    }
}