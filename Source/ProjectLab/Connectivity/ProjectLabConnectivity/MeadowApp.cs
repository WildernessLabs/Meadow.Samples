using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using ProjectLabConnectivity.Controllers;
using System.Threading.Tasks;

namespace ProjectLabConnectivity;

// Change ProjectLabCoreComputeApp to ProjectLabFeatherApp for ProjectLab v2
public class MeadowApp : ProjectLabCoreComputeApp
{
    public override async Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        Resolver.Log.Info($"Running on ProjectLab Hardware {Hardware.RevisionString}");

        var wifi = Hardware.ComputeModule.NetworkAdapters.Primary<IWiFiNetworkAdapter>();
        var ble = (Hardware.ComputeModule as F7MicroBase).BluetoothAdapter;

        var mainController = new MainController(Hardware, wifi, ble);
        await mainController.Initialize();
    }
}