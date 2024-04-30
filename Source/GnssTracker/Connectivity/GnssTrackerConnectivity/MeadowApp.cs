using GnssTrackerConnectivity.Controllers;
using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System.Threading.Tasks;

namespace GnssTrackerConnectivity;

public class MeadowApp : App<F7CoreComputeV2>
{
    public override async Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        var gnssTracker = GnssTracker.Create();
        Resolver.Log.Info($"Running on GnssTracker Hardware");

        var wifi = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();
        var ble = Device.BluetoothAdapter;

        var mainController = new MainController(gnssTracker, wifi, ble);
        await mainController.Initialize();
    }
}