using AmbientMonitor.Core;
using Meadow;
using Meadow.Devices;
using System.Threading.Tasks;

namespace AmbientMonitor.F7;

public class MeadowApp : ProjectLabCoreComputeApp
{
    private MainController? mainController;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        var hardware = new AmbientMonitorProjectLabHardware(Hardware);

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