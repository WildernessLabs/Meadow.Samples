using Meadow;
using Meadow.Devices;
using System.Threading.Tasks;
using WifiWeather.Core;
using WifiWeather.F7.Hardware;

namespace WifiWeather.F7;

// Change ProjectLabCoreComputeApp to ProjectLabFeatherApp for ProjectLab v2
public class MeadowApp : ProjectLabCoreComputeApp
{
    private MainController? mainController;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        var hardware = new WifiWeatherProjectLabHardware(Hardware);

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