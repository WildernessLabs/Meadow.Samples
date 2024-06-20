using Meadow;
using Meadow.Devices;
using System.Threading.Tasks;
using WifiWeather.Core;
using WifiWeather.F7.Hardware;

namespace WifiWeather.F7;

public class MeadowApp : App<F7CoreComputeV2>
{
    private MainController? mainController;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        var hardware = new WifiWeatherProjectLabHardware(Device);

        mainController = new MainController();
        mainController.Initialize(hardware);

        return Task.CompletedTask;
    }

    public override Task Run()
    {
        Resolver.Log.Info("Run...");

        _ = mainController!.Run();

        return Task.CompletedTask;
    }
}