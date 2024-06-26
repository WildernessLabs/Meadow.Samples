﻿using Meadow;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WifiWeather.Models;

namespace WifiWeather.Controllers;

public static class RestClientController
{
    static string climateDataUri = "http://api.openweathermap.org/data/2.5/weather";

    static RestClientController() { }

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
                var values = JsonSerializer.Deserialize<WeatherReading>(json);
                return values;
            }
            catch (TaskCanceledException)
            {
                Resolver.Log.Info("Request timed out.");
                return new WeatherReading();
            }
            catch (Exception e)
            {
                Resolver.Log.Info($"Request went sideways: {e.Message}");
                return new WeatherReading();
            }
        }
    }
}