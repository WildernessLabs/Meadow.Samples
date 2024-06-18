using AmbientMonitor.Core;
using AmbientMonitor.RPi.Hardware;
using Meadow;
using System.Threading.Tasks;

namespace AmbientMonitor.RPi;

internal class MeadowApp : App<RaspberryPi>
{
    private AmbientMonitorHardware hardware;
    private MainController? mainController;

    public bool SupportDisplay { get; set; } = false;

    public override Task Initialize()
    {
        hardware = new AmbientMonitorHardware(Device);
        mainController = new MainController();
        mainController.Initialize(hardware);

        return Task.CompletedTask;
    }

    public override Task Run()
    {
        mainController.Run();

        return Task.CompletedTask;
    }
}