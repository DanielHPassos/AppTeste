using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AppTeste.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get([FromQuery] string? location)
        {
            switch (location)
            {
                case "file":
                    string fileContent = System.IO.File.ReadAllText(Path.GetFullPath(@"DockerTeste/temperatura.json"));
                    var result = JsonSerializer.Deserialize<IEnumerable<WeatherForecast>>(fileContent);
                    return result;

                default:

                    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                    {
                        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                    })
            .ToArray();
            }

        }
    }
}