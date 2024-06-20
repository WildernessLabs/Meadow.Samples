using Meadow;
using System.Threading.Tasks;
using WifiWeather.Core;
using WifiWeather.RPi.Hardware;

namespace WifiWeather.RPi;

public class MeadowApp : App<RaspberryPi>
{
    private MainController? mainController;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        var hardware = new WifiWeatherHardware(Device);

        mainController = new MainController();
        mainController.Initialize(hardware);

        return Task.CompletedTask;
    }

    public override Task Run()
    {
        Resolver.Log.Info("Run...");

        mainController?.Run();

        return Task.CompletedTask;
    }
}