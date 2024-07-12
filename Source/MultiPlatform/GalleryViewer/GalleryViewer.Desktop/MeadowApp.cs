using GalleryViewer.Core;
using GalleryViewer.DesktopApp.Hardware;
using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Logging;
using System.Threading.Tasks;

namespace GalleryViewer.DesktopApp;

public class MeadowApp : App<Desktop>
{
    private MainController? mainController;

    public override Task Initialize()
    {
        Resolver.Log.AddProvider(new DebugLogProvider());

        Resolver.Log.Info("Initialize...");

        Device.Display.Resize(320, 240, 2);

        var hardware = new GalleryViewerHardware(Device);

        mainController = new MainController();
        mainController.Initialize(hardware);

        return Task.CompletedTask;
    }

    public override Task Run()
    {
        Resolver.Log.Info("Run...");

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