using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Web.Maple;
using Meadow.Hardware;
using MeadowMapleTemperature.Controllers;
using System;
using System.Threading.Tasks;

namespace MeadowMapleTemperature;

// public class MeadowApp : App<F7FeatherV1> <- If you have a Meadow F7v1.*
public class MeadowApp : App<F7FeatherV2>
{
    private IWiFiNetworkAdapter wifi;

    private LedController ledController;

    public override async Task Initialize()
    {
        ledController = new LedController();
        ledController.SetColor(Color.Red);

        var temperatureController = new TemperatureController();

        wifi = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();
        wifi.NetworkConnected += NetworkConnected;

        await wifi.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD, TimeSpan.FromSeconds(45));
    }

    private void NetworkConnected(INetworkAdapter sender, NetworkConnectionEventArgs args)
    {
        var mapleServer = new MapleServer(sender.IpAddress, 5417, true, logger: Resolver.Log);
        mapleServer.Start();

        ledController.SetColor(Color.Green);
    }
}