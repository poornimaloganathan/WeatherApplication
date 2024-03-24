using WeatherApplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace WeatherApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly WeatherService _weatherService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, WeatherService weatherService)
        {
            _logger = logger;
            _weatherService = weatherService;
        }

        [HttpGet("{cityName}")]
        public async Task<ActionResult<WeatherData>> GetWeatherDataByCity(string cityName)
        {
            try
            {
                WeatherData weatherData = await _weatherService.GetWeatherDataByCity(cityName);
                if (weatherData != null)
                {
                    return Ok(weatherData);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching weather data for {City}", cityName);
                return StatusCode(500, "An error occurred while fetching weather data.");
            }
        }
    }
}


