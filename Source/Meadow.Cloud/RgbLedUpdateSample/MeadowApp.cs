using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Hardware;
using Meadow.Peripherals.Leds;
using System.Threading.Tasks;

namespace RgbLedUpdateSample;

/*

meadow cloud login

meadow device provision

build application

meadow package create -a bin/Debug/netstandard2.1/postlink_bin

meadow package upload -p [path to .mpak file]

meadow collection list

meadow package publish -p your_package_id -c your_collection_id 

*/

// Change F7FeatherV2 to F7FeatherV1 for V1.x boards
public class MeadowApp : App<F7FeatherV2>
{
    private RgbPwmLed onboardLed;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        onboardLed = new RgbPwmLed(
            redPwmPin: Device.Pins.OnboardLedRed,
            greenPwmPin: Device.Pins.OnboardLedGreen,
            bluePwmPin: Device.Pins.OnboardLedBlue,
            CommonType.CommonAnode);
        onboardLed.StartBlink(Color.Red);

        var wifi = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();
        wifi.NetworkConnected += (s, e) =>
        {
            onboardLed.StartBlink(Color.Purple);
        };

        return base.Initialize();
    }

    public override Task Run()
    {
        Resolver.Log.Info("Run...");

        Resolver.UpdateService.ClearUpdates(); // uncomment to clear persisted info

        Resolver.UpdateService.UpdateAvailable += (updateService, info) =>
        {
            onboardLed.StartBlink(Color.Orange);
            Resolver.Log.Info("Update available!");

            Task.Run(async () =>
            {
                await Task.Delay(5000);
                updateService.RetrieveUpdate(info);
            });
        };

        Resolver.UpdateService.UpdateRetrieved += (updateService, info) =>
        {
            onboardLed.StartBlink(Color.Yellow);
            Resolver.Log.Info("Update retrieved!");

            Task.Run(async () =>
            {
                await Task.Delay(5000);
                updateService.ApplyUpdate(info);
            });
        };

        return Task.CompletedTask;
    }
}