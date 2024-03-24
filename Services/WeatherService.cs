using Newtonsoft.Json;

namespace WeatherApplication.Services
{
    public class WeatherService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly string _openWeatherMapApiKey;

        public WeatherService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _openWeatherMapApiKey = configuration["AppSettings:OpenWeatherMapApiKey"];
        }

        public async Task<WeatherData> GetWeatherDataByCity(string cityName)
        {
            try
            {
                string apiUrl = $"https://api.openweathermap.org/data/2.5/weather?q={cityName}&appid={_openWeatherMapApiKey}";

                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    OpenWeatherMapResponse weatherApiResponse = JsonConvert.DeserializeObject<OpenWeatherMapResponse>(json);
                    WeatherData weatherData = ConvertToWeatherData(weatherApiResponse);
                    return weatherData;
                }
                else
                {
                    // Handle error response
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private WeatherData ConvertToWeatherData(OpenWeatherMapResponse weatherApiResponse)
        {
            WeatherData weatherData = new WeatherData
            {
                City = weatherApiResponse.Name,
                Description = weatherApiResponse.Weather[0].Description,
                Temperature = new WeatherTemperature
                {
                    Celsius = weatherApiResponse.Main.Temp,
                    // No need to set Fahrenheit here as it's a calculated property
                }
            };
            return weatherData;
        }
    }

    public class OpenWeatherMapResponse
    {
        public string? Name { get; set; }
        public MainData? Main { get; set; }
        public WeatherDescription[]? Weather { get; set; }
    }

    public class MainData
    {
        public float Temp { get; set; }
    }

    public class WeatherDescription
    {
        public string? Description { get; set; }
    }

    public class WeatherData
    {
        public string? City { get; set; }
        public string? Description { get; set; }
        public WeatherTemperature Temperature { get; set; }   
    }

    public class WeatherTemperature
    {
        public float Celsius { get; set; }
        public float Fahrenheit => 32 + (float)(Celsius / 0.5556);
    }
}


