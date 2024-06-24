using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using ProjectLab_Command.Controllers;
using ProjectLab_Command.Hardware;
using System.Threading.Tasks;

namespace ProjectLab_Command;

// Change ProjectLabCoreComputeApp to ProjectLabFeatherApp for ProjectLab v2
public class MeadowApp : ProjectLabCoreComputeApp
{
    private MainController mainController;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        var hardware = new MeadowCloudCommandHardware(Hardware);
        var network = Hardware.ComputeModule.NetworkAdapters.Primary<IWiFiNetworkAdapter>();

        mainController = new MainController(hardware, network);
        mainController.Initialize();

        return Task.CompletedTask;
    }

    public override Task Run()
    {
        Resolver.Log.Info("Run...");

        mainController.Run();

        return Task.CompletedTask;
    }
}