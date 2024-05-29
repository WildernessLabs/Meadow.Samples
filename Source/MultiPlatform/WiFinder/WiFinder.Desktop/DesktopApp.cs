using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Logging;
using WiFinder.Core;

namespace WiFinder.Windows;

internal class MeadowApp : App<Desktop>
{
    private MainController mainController;

    public override Task Initialize()
    {
        // output log messages to the VS debug window
        Resolver.Log.AddProvider(new DebugLogProvider());

        var hardware = new DesktopHardware(Device);
        mainController = new MainController();
        return mainController.Initialize(hardware);
    }

    public override Task Run()
    {
        // this must be spawned in a worker because the UI needs the main thread
        _ = mainController.Run();

        ExecutePlatformDisplayRunner();

        return base.Run();
    }

    private void ExecutePlatformDisplayRunner()
    {
        if (Device.Display is SilkDisplay silkDisplay)
        {
            silkDisplay.Run();
            Environment.Exit(0);
        }
    }
}