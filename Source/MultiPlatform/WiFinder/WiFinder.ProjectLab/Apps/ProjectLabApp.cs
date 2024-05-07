using Meadow;
using Meadow.Devices;
using WiFinder.Core;

namespace WiFinder.F7;

public class ProjectLabApp : App<F7CoreComputeV2>
{
    private MainController mainController;

    public override Task Initialize()
    {
        var hardware = new ProjectLabHardware(Device);
        mainController = new MainController();
        return mainController.Initialize(hardware);
    }

    public override Task Run()
    {
        return mainController.Run();
    }
}
