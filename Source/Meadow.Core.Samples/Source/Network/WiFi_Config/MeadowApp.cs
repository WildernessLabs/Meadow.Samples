using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System.Threading.Tasks;

namespace MeadowApp
{
    // Change F7FeatherV2 to F7FeatherV1 for V1.x boards
    public class MeadowApp : App<F7FeatherV2>
    {
        public override Task Initialize()
        {
            Resolver.Log.Info("Initialize...");

            WireUpWiFiStatusEvents();

            return base.Initialize();
        }

        public override Task Run()
        {
            Resolver.Log.Info("Run...");



            return base.Run();
        }

        void WireUpWiFiStatusEvents()
        {
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
    }
}