using Microsoft.AspNetCore.Mvc;

namespace OpenApiSpectralDemo.Controllers;

/// <summary>
/// Weather forecast management controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    /// <summary>
    /// Retrieves weather forecasts for the next 5 days
    /// </summary>
    /// <returns>A list of weather forecasts</returns>
    /// <response code="200">Returns the weather forecasts</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpGet(Name = "GetWeatherForecast")]
    [ProducesResponseType(typeof(IEnumerable<WeatherForecast>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    /// <summary>
    /// Retrieves a specific weather forecast by day offset
    /// </summary>
    /// <param name="day">Day offset (1-5)</param>
    /// <returns>A weather forecast for the specified day</returns>
    /// <response code="200">Returns the weather forecast</response>
    /// <response code="400">If the day parameter is invalid</response>
    /// <response code="404">If the forecast is not found</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpGet("{day}", Name = "GetWeatherForecastByDay")]
    [ProducesResponseType(typeof(WeatherForecast), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public ActionResult<WeatherForecast> GetByDay(int day)
    {
        if (day < 1 || day > 5)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid day parameter",
                Detail = "Day must be between 1 and 5",
                Status = StatusCodes.Status400BadRequest
            });
        }

        var forecast = new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(day)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        };

        return Ok(forecast);
    }
}
