using GalleryViewer.Core;
using GalleryViewer.DesktopApp.Hardware;
using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Logging;
using System.Threading.Tasks;

namespace GalleryViewer.DesktopApp;

internal class MeadowApp : App<Meadow.Desktop>
{
    private MainController? mainController;

    public override Task Initialize()
    {
        // output log messages to the VS debug window
        Resolver.Log.AddProvider(new DebugLogProvider());

        var hardware = new GalleryViewerHardware(Device);
        mainController = new MainController();
        mainController.Initialize(hardware);

        return Task.CompletedTask;
    }

    public override Task Run()
    {
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