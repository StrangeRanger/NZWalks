using Microsoft.AspNetCore.Mvc;

namespace NZWalks.API.Controllers;

[ApiController]
[Route("weatherforecast")]
public class WeatherForecastController : ControllerBase
{
    private readonly string[] _summaries =
        ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

    [HttpGet]
    public IEnumerable<WeatherForecast> GetWeatherForecast()
    {
        return Enumerable.Range(1, 5)
            .Select(index => new WeatherForecast { Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                                                   TemperatureC = Random.Shared.Next(-20, 55),
                                                   Summary = _summaries[Random.Shared.Next(_summaries.Length)] })
            .ToArray();
    }
}

public record class WeatherForecast
{
    public DateOnly Date { get; init; }
    public int TemperatureC { get; init; }
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    public string? Summary { get; init; }
}