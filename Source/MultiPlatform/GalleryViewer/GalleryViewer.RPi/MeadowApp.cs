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
        return mainController.Initialize(hardware);
    }

    public override Task Run()
    {
        if (hardware.Display is GtkDisplay gtk)
        {
            _ = mainController.Run();
            gtk.Run();
        }

        return mainController.Run();
    }
}