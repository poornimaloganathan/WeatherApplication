using Microsoft.AspNetCore.Mvc.RazorPages;
using WeatherApplication.Services;


namespace WeatherApplication.Pages
{
    public class WeatherModel : PageModel
    {

            private readonly WeatherService _weatherService;

            public WeatherModel(WeatherService weatherService)
            {
                _weatherService = weatherService;
            }

            public WeatherData WeatherData { get; set; }

            public async Task OnGetAsync(string cityName)
            {
                if (!string.IsNullOrEmpty(cityName))
                {
                    WeatherData = await _weatherService.GetWeatherDataByCity(cityName);
                }
            }
        }
    }


