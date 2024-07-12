using GalleryViewer.Core;
using GalleryViewer.RPi.Hardware;
using Meadow;
using System.Threading.Tasks;

namespace GalleryViewer.RPi;

public class MeadowApp : App<RaspberryPi>
{
    private MainController? mainController;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        var hardware = new GalleryViewerHardware(Device);

        mainController = new MainController();
        mainController.Initialize(hardware);

        return Task.CompletedTask;
    }

    public override Task Run()
    {
        Resolver.Log.Info("Run...");

        return Task.CompletedTask;
    }
}