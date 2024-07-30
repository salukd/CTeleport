namespace WeatherForecast.Application.WeatherForecast.Queries;

public class GetWeatherForecastQueryValidator : AbstractValidator<GetWeatherForecastQuery>
{
    public GetWeatherForecastQueryValidator()
    {
        RuleFor(query => query.City)
            .NotEmpty().WithMessage("City is required.")
            .MaximumLength(100)
            .WithMessage("City name must not exceed 100 characters.");

        RuleFor(query => query.Date)
            .Must(BeWithinFiveDays).When(query => query.Date.HasValue)
            .WithMessage("Date must be within the next five days.");
    }

    private bool BeWithinFiveDays(DateTime? date)
    {
        if (!date.HasValue)
            return true;

        var today = DateTime.UtcNow;
        var fiveDaysFromNow = today.AddDays(5);

        return date.Value >= today.Date && date.Value.Date <= fiveDaysFromNow.Date;
    }
}