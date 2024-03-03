using Meadow;
using Meadow.Devices;
using System.Threading.Tasks;

namespace MultiPlatformApp;

public class F7CoreComputeV2App : App<F7CoreComputeV2>
{
    private OutputController outputController;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initializing F7CoreComputeV2 App...");

        outputController = new OutputController(Device.Pins.D09);

        return base.Initialize();
    }

    public override async Task Run()
    {
        Resolver.Log.Info("Running F7CoreComputeV2 App...");

        while (true)
        {
            outputController.SetOutputState(true);
            await Task.Delay(1000);
            outputController.SetOutputState(false);
            await Task.Delay(100);
        }
    }
}