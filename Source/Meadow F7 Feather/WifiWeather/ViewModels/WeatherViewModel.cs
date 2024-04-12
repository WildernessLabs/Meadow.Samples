using Meadow.Units;
using WifiWeather.Models;

namespace WifiWeather.ViewModels;

public class WeatherViewModel
{
    public int WeatherCode { get; set; }

    public int OutdoorTemperature { get; set; }

    public int IndoorTemperature { get; set; }

    public WeatherViewModel(WeatherReading outdoorConditions, Temperature indoorTemperature)
    {
        WeatherCode = outdoorConditions.weather[0].id;

        OutdoorTemperature = (int)(outdoorConditions.main.temp - 273);

        IndoorTemperature = (int)indoorTemperature.Celsius;
    }
}
