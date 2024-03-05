using WifiWeather.Models;

namespace WifiWeather.ViewModels
{
    public class WeatherViewModel
    {
        public int WeatherCode { get; set; }

        public int OutdoorTemperature { get; set; }

        public int FeelsLikeTemperature { get; set; }

        public int Pressure { get; set; }

        public int Humidity { get; set; }

        public decimal WindSpeed { get; set; }

        public int WindDirection { get; set; }

        public WeatherViewModel(WeatherReading outdoorConditions)
        {
            WeatherCode = outdoorConditions.weather[0].id;

            OutdoorTemperature = (int)(outdoorConditions.main.temp - 273);

            FeelsLikeTemperature = (int)(outdoorConditions.main.feels_like - 273);

            Pressure = outdoorConditions.main.pressure;

            Humidity = outdoorConditions.main.humidity;

            WindSpeed = outdoorConditions.wind.speed;

            WindDirection = outdoorConditions.wind.deg;
        }
    }
}