using GalleryViewer.Core;
using Meadow;
using Meadow.Devices;
using System.Threading.Tasks;

namespace GalleryViewer.F7;

public class MeadowApp : App<F7CoreComputeV2>
{
    private MainController? mainController;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        var hardware = new GalleryViewerProjectLabHardware(Device);

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