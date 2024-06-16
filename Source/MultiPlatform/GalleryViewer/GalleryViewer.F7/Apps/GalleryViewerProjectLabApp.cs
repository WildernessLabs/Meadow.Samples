using GalleryViewer.Core;
using Meadow;
using Meadow.Devices;
using System.Threading.Tasks;

namespace GalleryViewer.F7;

public class GalleryViewerProjectLabApp : App<F7CoreComputeV2>
{
    private MainController mainController;

    public override Task Initialize()
    {
        var hardware = new GalleryViewerProjectLabHardware(Device);
        mainController = new MainController();
        return mainController.Initialize(hardware);
    }

    public override Task Run()
    {
        return mainController.Run();
    }
}