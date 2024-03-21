using WifiWeather.Models;

namespace WifiWeather.ViewModels
{
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
                case int n when (n >= WeatherConstants.THUNDERSTORM_LIGHT_RAIN && n <= WeatherConstants.THUNDERSTORM_HEAVY_DRIZZLE):
                    resourceName = $"WifiWeather.w_storm.bmp";
                    break;
                case int n when (n >= WeatherConstants.DRIZZLE_LIGHT && n <= WeatherConstants.DRIZZLE_SHOWER):
                    resourceName = $"WifiWeather.w_drizzle.bmp";
                    break;
                case int n when (n >= WeatherConstants.RAIN_LIGHT && n <= WeatherConstants.RAIN_SHOWER_RAGGED):
                    resourceName = $"WifiWeather.w_rain.bmp";
                    break;
                case int n when (n >= WeatherConstants.SNOW_LIGHT && n <= WeatherConstants.SNOW_SHOWER_HEAVY):
                    resourceName = $"WifiWeather.w_snow.bmp";
                    break;
                case WeatherConstants.CLOUDS_CLEAR:
                    resourceName = $"WifiWeather.w_clear.bmp";
                    break;
                case int n when (n >= WeatherConstants.CLOUDS_FEW && n <= WeatherConstants.CLOUDS_OVERCAST):
                    resourceName = $"WifiWeather.w_cloudy.bmp";
                    break;
                default:
                    resourceName = $"WifiWeather.w_misc.bmp";
                    break;
            }

            return resourceName;
        }
    }
}