using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Foundation.ICs.IOExpanders;
using System;
using System.Threading.Tasks;
using WifiWeather.Services;
using WifiWeather.ViewModels;
using WifiWeather.Views;

public class MeadowApp : App<Desktop>
{
    private DisplayView _displayController;

    public override Task Initialize()
    {
        Resolver.Log.Info("Creating Outputs");

        var expander = FtdiExpanderCollection.Devices[0];

        var display = new Ili9488
        (
            spiBus: expander.CreateSpiBus(),
            chipSelectPin: expander.Pins.C0,
            dcPin: expander.Pins.C2,
            resetPin: expander.Pins.C1
        );

        _displayController = new DisplayView(display);

        return Task.CompletedTask;
    }

    async Task GetTemperature()
    {
        // Get outdoor conditions
        var outdoorConditions = await WeatherService.GetWeatherForecast();

        // Format indoor/outdoor conditions data
        var model = new WeatherViewModel(outdoorConditions);

        // Send formatted data to display to render
        _displayController.UpdateDisplay(
            weatherIcon: model.WeatherIcon,
            temperature: $"{model.OutdoorTemperature:n0}°C",
            humidity: $"{model.Humidity:n0}%",
            pressure: $"{model.Pressure:n0}hPa",
            feelsLike: $"{model.FeelsLikeTemperature:n0}°C",
            windDirection: $"{model.WindDirection:n0}°",
            windSpeed: $"{model.WindSpeed:n0}m/s");
    }

    public override async Task Run()
    {
        await GetTemperature();

        while (true)
        {
            if (DateTime.Now.Minute == 0 && DateTime.Now.Second == 0)
            {
                await GetTemperature();
            }

            _displayController.UpdateDateTime();
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }

    public static async Task Main(string[] args)
    {
        await MeadowOS.Start(args);
    }
}