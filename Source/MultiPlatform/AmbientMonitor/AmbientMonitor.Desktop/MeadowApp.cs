using AmbientMonitor.Core;
using AmbientMonitor.DesktopApp.Hardware;
using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Logging;
using System.Threading.Tasks;

namespace AmbientMonitor.DesktopApp;

internal class MeadowApp : App<Desktop>
{
    private MainController? mainController;

    public override Task Initialize()
    {
        Resolver.Log.AddProvider(new DebugLogProvider());

        Resolver.Log.Info("Initialize...");

        var hardware = new AmbientMonitorHardware(Device);

        mainController = new MainController();
        mainController.Initialize(hardware);

        return Task.CompletedTask;
    }

    public override Task Run()
    {
        Resolver.Log.Info("Run...");

        // this must be spawned in a worker because the UI needs the main thread
        _ = mainController?.Run();

        ExecutePlatformDisplayRunner();

        return Task.CompletedTask;
    }

    private void ExecutePlatformDisplayRunner()
    {
        if (Device.Display is SilkDisplay silkDisplay)
        {
            silkDisplay.Run();
        }
    }
}