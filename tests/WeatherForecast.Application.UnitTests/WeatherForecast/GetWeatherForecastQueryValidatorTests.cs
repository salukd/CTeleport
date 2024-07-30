using FluentValidation.TestHelper;
using WeatherForecast.Application.WeatherForecast.Queries;

namespace WeatherForecast.Application.UnitTests.WeatherForecast;

public class GetWeatherForecastQueryValidatorTests
{
     private readonly GetWeatherForecastQueryValidator _validator = new();

     [Fact]
    public void Validate_WhenCityIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var query = new GetWeatherForecastQuery("", null);

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(q => q.City)
            .WithErrorMessage("City is required.");
    }

    [Fact]
    public void Validate_WhenCityExceeds100Characters_ShouldHaveValidationError()
    {
        // Arrange
        var longCityName = new string('A', 101);
        var query = new GetWeatherForecastQuery(longCityName, null);

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(q => q.City)
            .WithErrorMessage("City name must not exceed 100 characters.");
    }

    [Fact]
    public void Validate_WhenCityIsValid_ShouldNotHaveValidationError()
    {
        // Arrange
        var query = new GetWeatherForecastQuery("New York", null);

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveValidationErrorFor(q => q.City);
    }

    [Fact]
    public void Validate_WhenDateIsNull_ShouldNotHaveValidationError()
    {
        // Arrange
        var query = new GetWeatherForecastQuery("London", null);

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveValidationErrorFor(q => q.Date);
    }

    [Fact]
    public void Validate_WhenDateIsWithinFiveDays_ShouldNotHaveValidationError()
    {
        // Arrange
        var futureDate = DateTime.UtcNow.AddDays(3);
        var query = new GetWeatherForecastQuery("Paris", futureDate);

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveValidationErrorFor(q => q.Date);
    }

    [Fact]
    public void Validate_WhenDateIsMoreThanFiveDaysAhead_ShouldHaveValidationError()
    {
        // Arrange
        var farFutureDate = DateTime.UtcNow.AddDays(6);
        var query = new GetWeatherForecastQuery("Berlin", farFutureDate);

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(q => q.Date)
            .WithErrorMessage("Date must be within the next five days.");
    }

    [Fact]
    public void Validate_WhenDateIsInPast_ShouldHaveValidationError()
    {
        // Arrange
        var pastDate = DateTime.UtcNow.AddDays(-1);
        var query = new GetWeatherForecastQuery("Tokyo", pastDate);

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(q => q.Date)
            .WithErrorMessage("Date must be within the next five days.");
    }

    [Fact]
    public void Validate_WhenDateIsToday_ShouldNotHaveValidationError()
    {
        // Arrange
        var today = DateTime.UtcNow.Date;
        var query = new GetWeatherForecastQuery("Rome", today);

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveValidationErrorFor(q => q.Date);
    }

    [Fact]
    public void Validate_WhenDateIsExactlyFiveDaysAhead_ShouldNotHaveValidationError()
    {
        // Arrange
        var fiveDaysAhead = DateTime.UtcNow.Date.AddDays(5);
        var query = new GetWeatherForecastQuery("Madrid", fiveDaysAhead);

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveValidationErrorFor(q => q.Date);
    }
}