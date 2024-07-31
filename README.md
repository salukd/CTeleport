# Weather Forecast Service

## Description

The Weather Forecast Service is an ASP.NET Core Web API that provides weather forecasts for specified cities. It integrates with the OpenWeatherMap API to fetch weather data and uses Redis for caching to improve performance and reduce external API calls.

## Key Features

- Fetch 5-day weather forecasts for any city
- Cache results in Redis to improve response times and reduce API usage
- Health checks for both the OpenWeatherMap API and Redis connection
- Error handling for various scenarios (city not found, API key issues, rate limiting, etc.)
- Resilience patterns implemented using Polly:
  - Rate Limiting
  - Circuit Breaker
  - Retry Policies

## Technologies and Libraries Used

- ASP.NET Core 8.0
- C# 12.0
- Redis (via StackExchange.Redis)
- Swashbuckle.AspNetCore (for Swagger/OpenAPI)
- RestEase (for OpenWeatherMap API client)
- AspNetCore.HealthChecks.Redis
- Polly (for resilience and transient fault handling)
- FluentValidation (for input validation)
- Mediatr
- Polly (for resilience and transient fault handling)

## Prerequisites

- .NET 8.0 SDK
- Docker and Docker Compose
- OpenWeatherMap API key

## Setup and Configuration

1. Clone the repository:
   ```
   git clone https://github.com/salukd/CTeleport.git
   cd CTeleport
   ```

2. Set up your `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "Redis": "your-redis-connection-string"
     },
     "OpenWeatherMapApi": {
       "ApiKey": "your-api-key",
       "BaseUrl": "http://api.openweathermap.org/data/2.5"
     }
   }
   ```

3. Restore NuGet packages:
   ```
   dotnet restore
   ```

## Running the Service

### Using Docker Compose

1. Build and run the service using Docker Compose:
   ```
   docker-compose up --build
   ```

2. The API will be available at `http://localhost:8080/swagger/index.html`.

### Using .NET CLI

1. Build the project:
   ```
   dotnet build
   ```

2. Run the service:
   ```
   dotnet run --project src/WeatherForecast.Api.Rest
   ```

3. The API will be available at `https://localhost:5001` (or `http://localhost:5000`).

4. Access the Swagger UI at `https://localhost:5001/swagger` to explore and test the API.

## API Endpoints

- `GET /api/v1/WeatherForecast?city={cityName}&date={date}`: Get weather forecast for a specific city and date.
- `GET /health`: Check the health status of the service and its dependencies.

## Health Checks

The service includes health checks for:
- Redis connection
- OpenWeatherMap API availability

Access the health check endpoint at `/health` to view the status of these dependencies.

## Testing

To run the unit tests:

```
dotnet test
