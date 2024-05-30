using Meadow.Foundation.Serialization;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WifiWeatherClock.Models;

namespace WifiWeatherClock.Services;

public static class WeatherService
{
    static string climateDataUri = "http://api.openweathermap.org/data/2.5/weather";
    static string city = $"q=[CITY]";
    static string apiKey = $"appid=[API KEY]";

    static WeatherService() { }

    public static async Task<WeatherReading> GetWeatherForecast()
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                client.Timeout = new TimeSpan(0, 5, 0);

                HttpResponseMessage response = await client.GetAsync($"{climateDataUri}?{city}&{apiKey}");

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