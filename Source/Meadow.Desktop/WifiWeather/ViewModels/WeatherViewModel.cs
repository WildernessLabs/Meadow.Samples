using WifiWeather.Models;
using WifiWeather.Utils;

namespace WifiWeather.ViewModels;

public class WeatherViewModel
{
    public int WeatherCode { get; set; }

    public string WeatherIcon { get; set; }

    public int OutdoorTemperature { get; set; }

    public int FeelsLikeTemperature { get; set; }

    public int Pressure { get; set; }

    public int Humidity { get; set; }

    public decimal WindSpeed { get; set; }

    public int WindDirection { get; set; }

    public WeatherViewModel(WeatherReading outdoorConditions)
    {
        WeatherCode = outdoorConditions.weather[0].id;

        WeatherIcon = LoadResource(WeatherCode);

        OutdoorTemperature = (int)(outdoorConditions.main.temp - 273);

        FeelsLikeTemperature = (int)(outdoorConditions.main.feels_like - 273);

        Pressure = outdoorConditions.main.pressure;

        Humidity = outdoorConditions.main.humidity;

        WindSpeed = outdoorConditions.wind.speed;

        WindDirection = outdoorConditions.wind.deg;
    }

    string LoadResource(int weatherCode)
    {
        string resourceName;

        switch (weatherCode)
        {
            case int n when (n >= WeatherCodeConstants.THUNDERSTORM_LIGHT_RAIN && n <= WeatherCodeConstants.THUNDERSTORM_HEAVY_DRIZZLE):
                resourceName = $"WifiWeather.Resources.w_storm.bmp";
                break;
            case int n when (n >= WeatherCodeConstants.DRIZZLE_LIGHT && n <= WeatherCodeConstants.DRIZZLE_SHOWER):
                resourceName = $"WifiWeather.Resources.w_drizzle.bmp";
                break;
            case int n when (n >= WeatherCodeConstants.RAIN_LIGHT && n <= WeatherCodeConstants.RAIN_SHOWER_RAGGED):
                resourceName = $"WifiWeather.Resources.w_rain.bmp";
                break;
            case int n when (n >= WeatherCodeConstants.SNOW_LIGHT && n <= WeatherCodeConstants.SNOW_SHOWER_HEAVY):
                resourceName = $"WifiWeather.Resources.w_snow.bmp";
                break;
            case WeatherCodeConstants.CLOUDS_CLEAR:
                resourceName = $"WifiWeather.Resources.w_clear.bmp";
                break;
            case int n when (n >= WeatherCodeConstants.CLOUDS_FEW && n <= WeatherCodeConstants.CLOUDS_OVERCAST):
                resourceName = $"WifiWeather.Resources.w_cloudy.bmp";
                break;
            default:
                resourceName = $"WifiWeather.Resources.w_misc.bmp";
                break;
        }

        return resourceName;
    }
}