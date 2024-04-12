using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Web.Maple;
using Meadow.Hardware;
using MeadowMapleLed.Controller;
using System;
using System.Threading.Tasks;

namespace MeadowMapleLed;

// public class MeadowApp : App<F7FeatherV1> <- If you have a Meadow F7v1.*
public class MeadowApp : App<F7FeatherV2>
{
    MapleServer mapleServer;

    public override async Task Initialize()
    {
        LedController.Instance.SetColor(Color.Red);

        var wifi = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();
        wifi.NetworkConnected += NetworkConnected;

        await wifi.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD, TimeSpan.FromSeconds(45));
    }

    private void NetworkConnected(INetworkAdapter sender, NetworkConnectionEventArgs args)
    {
        mapleServer = new MapleServer(sender.IpAddress, 5417, true, logger: Resolver.Log);
        mapleServer.Start();

        LedController.Instance.SetColor(Color.Green);
    }
}