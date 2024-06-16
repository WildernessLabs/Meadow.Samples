using GalleryViewer.Core;
using Meadow;
using Meadow.Devices;
using System.Threading.Tasks;

namespace GalleryViewer.F7;

public class GalleryViewerF7FeatherApp : App<F7FeatherV2>
{
    private MainController mainController;

    public override Task Initialize()
    {
        var hardware = new GalleryViewerF7FeatherHardware(Device);
        mainController = new MainController();
        return mainController.Initialize(hardware);
    }

    public override Task Run()
    {
        return mainController.Run();
    }
}