using Meadow;
using Meadow.Devices;
using Meadow.Gateway.WiFi;
using Meadow.Hardware;
using System.Threading.Tasks;

namespace AntennaSwitching;

public class MeadowApp : App<F7FeatherV2>
{
    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize hardware...");

        return Task.CompletedTask;
    }

    public override async Task Run()
    {
        var wifi = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();

        // enumerate the public WiFi channels
        await ScanForAccessPoints(wifi);

        // get the current antenna
        Resolver.Log.Info($"Current antenna in use: {wifi.CurrentAntenna}");

        // change to the external antenna
        Resolver.Log.Info($"Switching to external antenna.");
        wifi.SetAntenna(AntennaType.External, persist: false);
        Resolver.Log.Info($"Current antenna in use: {wifi.CurrentAntenna}");

        // enumerate WiFis again on the new antenna
        await ScanForAccessPoints(wifi);
    }

    async Task ScanForAccessPoints(IWiFiNetworkAdapter adapter)
    {
        Resolver.Log.Info("Getting list of access points.");

        var networks = await adapter.Scan();
        if (networks.Count > 0)
        {
            Resolver.Log.Info("|-------------------------------------------------------------|---------|");
            Resolver.Log.Info("|         Network Name             | RSSI |       BSSID       | Channel |");
            Resolver.Log.Info("|-------------------------------------------------------------|---------|");
            foreach (WifiNetwork accessPoint in networks)
            {
                Resolver.Log.Info($"| {accessPoint.Ssid,-32} | {accessPoint.SignalDbStrength,4} | {accessPoint.Bssid,17} |   {accessPoint.ChannelCenterFrequency,3}   |");
            }
        }
        else
        {
            Resolver.Log.Info($"No access points detected.");
        }
    }
}