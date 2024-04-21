using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using ProjectLabConnectivity.Controllers;
using System.Threading.Tasks;

namespace ProjectLabConnectivity;

// Change F7CoreComputeV2 to F7FeatherV2 for ProjectLab v2
public class MeadowApp : App<F7CoreComputeV2>
{
    public override async Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        var projectLab = ProjectLab.Create();
        Resolver.Log.Info($"Running on ProjectLab Hardware {projectLab.RevisionString}");

        var wifi = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();
        var ble = Device.BluetoothAdapter;

        var mainController = new MainController(projectLab, wifi, ble);
        await mainController.Initialize();
    }
}