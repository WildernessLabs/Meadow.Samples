using Meadow.Devices;
using WiFinder.Core;

namespace WiFinder.F7;

// Change ProjectLabCoreComputeApp to ProjectLabFeatherApp for ProjectLab v2
public class MeadowApp : ProjectLabCoreComputeApp
{
    private MainController mainController;

    public override Task Initialize()
    {
        var hardware = new ProjectLabHardware(Hardware);
        mainController = new MainController();
        return mainController.Initialize(hardware);
    }

    public override Task Run()
    {
        return mainController.Run();
    }
}
