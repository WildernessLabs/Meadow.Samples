using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using ProjectLab_AzureIoTHub.Controllers;
using ProjectLab_AzureIoTHub.Hardware;
using System.Threading.Tasks;

namespace ProjectLab_AzureIoTHub;

// Change ProjectLabCoreComputeApp to ProjectLabFeatherApp for ProjectLab v2
public class MeadowApp : ProjectLabCoreComputeApp
{
    private MainController mainController;

    public override async Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        var hardware = new MeadowAzureIoTHubHardware(Hardware);
        var network = Hardware.ComputeModule.NetworkAdapters.Primary<IWiFiNetworkAdapter>();

        mainController = new MainController(hardware, network);
        await mainController.Initialize();
    }

    public override async Task Run()
    {
        Resolver.Log.Info("Run...");

        await mainController.Run();
    }
}