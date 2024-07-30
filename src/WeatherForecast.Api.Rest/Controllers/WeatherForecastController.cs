using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WeatherForecast.Api.Rest.Controllers;

[ApiVersion("1.0")]
public class WeatherForecastController : ApiController
{
    public WeatherForecastController(ISender mediator) : base(mediator)
    {
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {

        return Ok();
    }
}