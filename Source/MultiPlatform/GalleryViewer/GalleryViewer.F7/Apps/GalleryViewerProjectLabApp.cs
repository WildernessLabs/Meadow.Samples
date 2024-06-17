using GalleryViewer.Core;
using Meadow;
using Meadow.Devices;
using System.Threading.Tasks;

namespace GalleryViewer.F7;

public class GalleryViewerProjectLabApp : App<F7CoreComputeV2>
{
    private MainController? mainController;

    public override Task Initialize()
    {
        var hardware = new GalleryViewerProjectLabHardware(Device);
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