using AmbientMonitor.Core;
using Meadow;
using Meadow.Devices;
using System.Threading.Tasks;

namespace AmbientMonitor.F7;

public class MeadowApp : App<F7CoreComputeV2>
{
    private MainController? mainController;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        var hardware = new AmbientMonitorProjectLabHardware(Device);

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