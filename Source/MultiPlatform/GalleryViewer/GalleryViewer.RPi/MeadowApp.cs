using GalleryViewer.Core;
using GalleryViewer.RPi.Hardware;
using Meadow;
using Meadow.Foundation.Displays;
using System.Threading.Tasks;

namespace GalleryViewer.RPi;

internal class MeadowApp : App<RaspberryPi>
{
    private GalleryViewerHardware? hardware;
    private MainController? mainController;

    public bool SupportDisplay { get; set; } = false;

    public override Task Initialize()
    {
        hardware = new GalleryViewerHardware(Device, SupportDisplay);
        mainController = new MainController();
        mainController.Initialize(hardware);

        return Task.CompletedTask;
    }

    public override Task Run()
    {
        mainController?.Run();

        if (hardware.Display is GtkDisplay gtk)
        {
            gtk.Run();
        }

        return Task.CompletedTask;
    }
}