using Meadow;
using Meadow.Hardware;
using System.Linq;
using System.Threading.Tasks;

namespace WiFi_Basics
{
    public class MeadowApp : App<Desktop>
    {
        public static async Task Main(string[] args)
        {
            await MeadowOS.Start(args);
        }

        public override async Task Run()
        {
            Resolver.Log.Info($"Meadow.Windows Network Sample");

            Resolver.Log.Info($"{Device.NetworkAdapters.Count} network adapters detected");
            Resolver.Log.Info($"----------------------------");

            foreach (var adapter in Device.NetworkAdapters)
            {
                Resolver.Log.Info($"  {adapter.Name}  {adapter.IpAddress}");
            }

            Resolver.Log.Info($"WiFi info");
            Resolver.Log.Info($"----------------------------");
            foreach (var wifi in Device.NetworkAdapters.Where(i => i is IWiFiNetworkAdapter).Cast<IWiFiNetworkAdapter>())
            {
                Resolver.Log.Info($"  {wifi.Name}  {wifi.IpAddress}");

                var networks = await wifi.Scan();
                foreach (var network in networks)
                {
                    Resolver.Log.Info($"     {network.Ssid}: {network.SignalDbStrength}");
                }
            }
        }
    }
}
