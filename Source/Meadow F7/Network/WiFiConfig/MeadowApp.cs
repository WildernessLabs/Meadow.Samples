using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System.Threading.Tasks;

namespace WiFi_Config;

public class CoreComputeApp : MeadowApp<F7CoreComputeV2> { }
public class F7FeatherV2App : MeadowApp<F7FeatherV2> { }
public class F7FeatherV1App : MeadowApp<F7FeatherV1> { }

public class MeadowApp<T> : App<T>
    where T : F7MicroBase
{
    public override async Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        // get the wifi adapter
        var wifi = Resolver.Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();
        // set initial state
        if (wifi.IsConnected)
        {
            Resolver.Log.Info("Already connected to WiFi.");
        }
        else
        {
            Resolver.Log.Info("Not connected to WiFi yet.");
        }
        // connect event
        wifi.NetworkConnected += (networkAdapter, networkConnectionEventArgs) =>
        {
            Resolver.Log.Info($"Joined network - IP Address: {networkAdapter.IpAddress}");
        };
        // disconnect event
        wifi.NetworkDisconnected += (o, e) =>
        {
            Resolver.Log.Info($"Network disconnected.");
        };
    }

    public override Task Run()
    {
        Resolver.Log.Info("Run...");

        return base.Run();
    }
}