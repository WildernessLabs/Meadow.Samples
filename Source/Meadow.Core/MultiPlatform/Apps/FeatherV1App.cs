using Meadow;
using Meadow.Devices;
using System.Threading.Tasks;

namespace MultiPlatformApp;

public class FeatherV1App : App<F7FeatherV1>
{
    private OutputController outputController;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initializing F7FeatherV1 App...");

        outputController = new OutputController(Device.Pins.OnboardLedGreen);

        return base.Initialize();
    }

    public override async Task Run()
    {
        Resolver.Log.Info("Running F7FeatherV1 App...");

        while (true)
        {
            outputController.SetOutputState(true);
            await Task.Delay(1000);
            outputController.SetOutputState(false);
            await Task.Delay(100);
        }
    }
}
