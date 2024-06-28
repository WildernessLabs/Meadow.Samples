using AmbientRoomMonitor.Hardware;
using Meadow;
using Meadow.Devices;
using System.Threading.Tasks;

namespace AmbientRoomMonitor;

// Change ProjectLabCoreComputeApp to ProjectLabFeatherApp for ProjectLab v2
public class MeadowApp : ProjectLabCoreComputeApp
{
    private MainController mainController;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        mainController = new MainController(new AmbientRoomMonitorHardware(Hardware));
        mainController.Initialize();

        return Task.CompletedTask;
    }

    public override Task Run()
    {
        Resolver.Log.Info("Run...");

        mainController.Run();

        return Task.CompletedTask;
    }
}