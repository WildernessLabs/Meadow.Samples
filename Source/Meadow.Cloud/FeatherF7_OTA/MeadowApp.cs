using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Peripherals.Leds;
using Meadow.Update;
using System.Threading;
using System.Threading.Tasks;

namespace FeatherF7_OTA;

public class MeadowApp : App<F7FeatherV2>
{
    /*
    
    OTA instructions:

    1. Change color value below

    2. Open a Terminal (VS2022 - View -> Terminal) and create an mpak file: 

        meadow cloud package create --name <filename>.mpak

    3. Upload the mpak file to Meadow.Cloud:

        meadow cloud package upload bin\Release\netstandard2.1\mpak\<filename>.mpak

    4. Publish the mpak uploaded to roll out an OTA Update:

        meadow cloud package publish <package Id> --collectionId <collection Id>, or
    
        Go to Meadow.Cloud (https://www.meadowcloud.co/) -> Packages, click Publish on the .mpak uploaded

    5. If/When Meadow device resets, you can still check its console output with the command:

        meadow listen

    */

    RgbPwmLed onboardLed;
    Color color = Color.Red;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        onboardLed = new RgbPwmLed(
            redPwmPin: Device.Pins.OnboardLedRed,
            greenPwmPin: Device.Pins.OnboardLedGreen,
            bluePwmPin: Device.Pins.OnboardLedBlue,
            CommonType.CommonAnode);

        var updateService = Resolver.UpdateService;

        updateService.StateChanged += OnUpdateStateChanged;

        updateService.RetrieveProgress += OnUpdateProgress;

        updateService.UpdateAvailable += OnUpdateAvailable;

        updateService.UpdateRetrieved += OnUpdateRetrieved;

        var cloudService = Resolver.MeadowCloudService;

        cloudService.ConnectionStateChanged += OnCloudStateChanged;

        return Task.CompletedTask;
    }

    private void OnUpdateStateChanged(object sender, UpdateState e)
    {
        Resolver.Log.Info($"UpdateState {e}");
    }

    private void OnUpdateProgress(IUpdateService updateService, UpdateInfo info, CancellationTokenSource token)
    {
        short percentage = (short)(((double)info.DownloadProgress / info.FileSize) * 100);

        Resolver.Log.Info($"Downloading... {percentage}%");
    }

    private async void OnUpdateAvailable(IUpdateService updateService, UpdateInfo info, CancellationTokenSource token)
    {
        Resolver.Log.Info($"Update available!");

        _ = onboardLed.StartBlink(Color.Magenta);
    }

    private async void OnUpdateRetrieved(IUpdateService updateService, UpdateInfo info, CancellationTokenSource token)
    {
        Resolver.Log.Info($"Update retrieved!");

        _ = onboardLed.StartBlink(Color.Cyan);
    }

    private void OnCloudStateChanged(object sender, CloudConnectionState e)
    {
        Resolver.Log.Info($"CloudConnectionState: {e}");
    }

    public override Task Run()
    {
        Resolver.Log.Info("Run...");

        onboardLed.StartPulse(color);

        return Task.CompletedTask;
    }
}