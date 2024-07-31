FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Development
WORKDIR /src
COPY ["src/WeatherForecast.Api.Rest/WeatherForecast.Api.Rest.csproj", "src/WeatherForecast.Api.Rest/"]
COPY ["src/WeatherForecast.Application/WeatherForecast.Application.csproj", "src/WeatherForecast.Application/"]
COPY ["src/WeatherForecast.Domain/WeatherForecast.Domain.csproj", "src/WeatherForecast.Domain/"]
COPY ["src/WeatherForecast.Infrastructure/WeatherForecast.Infrastructure.csproj", "src/WeatherForecast.Infrastructure/"]
RUN dotnet restore "src/WeatherForecast.Api.Rest/WeatherForecast.Api.Rest.csproj"
COPY . .
WORKDIR "/src/src/WeatherForecast.Api.Rest"
RUN dotnet build "WeatherForecast.Api.Rest.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Development
RUN dotnet publish "WeatherForecast.Api.Rest.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WeatherForecast.Api.Rest.dll"]