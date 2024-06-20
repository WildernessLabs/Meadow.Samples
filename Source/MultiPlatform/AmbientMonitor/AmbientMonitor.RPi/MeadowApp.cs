using AmbientMonitor.Core;
using AmbientMonitor.RPi.Hardware;
using Meadow;
using System.Threading.Tasks;

namespace AmbientMonitor.RPi;

public class MeadowApp : App<RaspberryPi>
{
    private MainController? mainController;

    public bool SupportDisplay { get; set; } = false;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        var hardware = new AmbientMonitorHardware(Device);
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