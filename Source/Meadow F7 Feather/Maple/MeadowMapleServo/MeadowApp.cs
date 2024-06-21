using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Web.Maple;
using Meadow.Hardware;
using Meadow.Units;
using MeadowMapleServo.Controllers;
using System;
using System.Threading.Tasks;

namespace MeadowMapleServo;

// public class MeadowApp : App<F7FeatherV1> <- If you have a Meadow F7v1.*
public class MeadowApp : App<F7FeatherV2>
{
    private RgbPwmLed onboardLed;

    private IWiFiNetworkAdapter wifi;

    private ServoController servoController;

    public override async Task Initialize()
    {
        onboardLed = new RgbPwmLed(
            redPwmPin: Device.Pins.OnboardLedRed,
            greenPwmPin: Device.Pins.OnboardLedGreen,
            bluePwmPin: Device.Pins.OnboardLedBlue);
        onboardLed.SetColor(Color.Red);

        servoController = new ServoController();
        servoController.RotateTo(Angle.Zero);

        wifi = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();
        wifi.NetworkConnected += NetworkConnected;

        await wifi.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD, TimeSpan.FromSeconds(45));
    }

    private void NetworkConnected(INetworkAdapter sender, NetworkConnectionEventArgs args)
    {
        var mapleServer = new MapleServer(sender.IpAddress, 5417, true, logger: Resolver.Log);
        mapleServer.Start();

        onboardLed.SetColor(Color.Green);
    }
}