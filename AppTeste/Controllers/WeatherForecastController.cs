using AppTeste.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
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
        private readonly ConfigSettings configuration;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ConfigSettings configuration)
        {
            _logger = logger;
            this.configuration = configuration;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IActionResult> Get([FromQuery] string? c)
        {
            string cc = c ?? configuration.ConnectionString;
            try
            {

                using (var connection = new SqlConnection(cc))
                {
                    await connection.OpenAsync();
                }
            }
            catch (Exception ex)
            {
                return Ok($"ConnectionString Passada: {cc} - {ex.ToString()}");
            }

            return Ok("Sucesso");
        }
    }
}