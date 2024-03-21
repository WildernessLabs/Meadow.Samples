using Meadow;
using Meadow.Foundation.Leds;
using Meadow.Hardware;
using System.Threading.Tasks;

namespace Blinky;

public class MeadowApp : App<RaspberryPi>
{
    private RgbLed? rgbLed;
    private IDigitalOutputPort onboardLed = default;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        onboardLed = Device.Pins.ACT_LED.CreateDigitalOutputPort();

        return base.Initialize();
    }

    public override async Task Run()
    {
        Resolver.Log.Info("Run...");

        var state = false;

        while (true)
        {
            state = !state;
            Resolver.Log.Info($"Set State = {state}");
            onboardLed.State = state;
            Resolver.Log.Info($"Read State = {onboardLed.State}");

            await Task.Delay(1000);
        }
    }
}