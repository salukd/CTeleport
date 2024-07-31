using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using WeatherForecast.Api.Rest.Health;
using WeatherForecast.Api.Rest.Swagger;

var builder = WebApplication.CreateBuilder(args);


builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("secrets.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();


builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Weather Forecast API", Version = "v1" });
    c.DocumentFilter<HealthCheckFilter>();
    c.EnableAnnotations();
});

builder.Services.AddPresentation()
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration);


builder.Services.AddHealthChecks()
    .AddRedis(builder.Configuration.GetConnectionString("Cache")!,  tags: new[] { "ready" })
    .AddCheck<OpenWeatherMapHealthCheck>("openweathermap_api",  tags: new[] { "live" });

var app = builder.Build();

app.UseProblemDetails();

app.UseExceptionHandler(options => { });

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHsts();
app.UseHttpsRedirection();

app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapControllers();

app.Run();