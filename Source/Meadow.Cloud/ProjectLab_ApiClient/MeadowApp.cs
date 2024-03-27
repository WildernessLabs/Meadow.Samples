using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using ProjectLab_ApiClient.Controllers;
using ProjectLab_ApiClient.Hardware;
using System.Threading.Tasks;

namespace ProjectLab_ApiClient;

// Change F7CoreComputeV2 to F7FeatherV2 (or F7FeatherV1) for Feather boards
public class MeadowApp : App<F7CoreComputeV2>
{
    MainController mainController;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        var hardware = new MeadowCloudClientHardware();
        var network = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();

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