using Meadow.Foundation.Serialization;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WifiWeather.Models;

namespace WifiWeather.Services;

public static class WeatherService
{
    static string climateDataUri = "http://api.openweathermap.org/data/2.5/weather";

    static WeatherService() { }

    public static async Task<WeatherReading> GetWeatherForecast()
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                client.Timeout = new TimeSpan(0, 5, 0);

                HttpResponseMessage response = await client.GetAsync($"{climateDataUri}?q={Secrets.WEATHER_CITY}&appid={Secrets.WEATHER_API_KEY}");

                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                var values = MicroJson.Deserialize<WeatherReading>(json);
                return values;
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("Request timed out.");
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Request went sideways: {e.Message}");
                return null;
            }
        }
    }
}