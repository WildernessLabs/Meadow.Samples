using GalleryViewer.Core;
using GalleryViewer.RPi.Hardware;
using Meadow;
using System.Threading.Tasks;

namespace GalleryViewer.RPi;

internal class MeadowApp : App<RaspberryPi>
{
    private MainController? mainController;

    public override Task Initialize()
    {
        var hardware = new GalleryViewerHardware(Device);
        mainController = new MainController();
        mainController.Initialize(hardware);

        return Task.CompletedTask;
    }

    public override Task Run()
    {
        mainController?.Run();

        return Task.CompletedTask;
    }
}