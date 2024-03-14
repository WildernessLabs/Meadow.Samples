using Meadow;
using Meadow.Foundation.Leds;

namespace DigitalOutputs_Sample;

public class MeadowApp : LinuxApp<RaspberryPi>
{
    private List<Led>? leds;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        leds = new List<Led>
            {
                new Led(Device.Pins.GPIO2),
                new Led(Device.Pins.GPIO3),
                new Led(Device.Pins.GPIO4),
                new Led(Device.Pins.GPIO17),
                new Led(Device.Pins.GPIO27),
                new Led(Device.Pins.GPIO22),
                new Led(Device.Pins.GPIO10),
                new Led(Device.Pins.GPIO9),
                new Led(Device.Pins.GPIO11),
                new Led(Device.Pins.GPIO5),
                new Led(Device.Pins.GPIO6),
                new Led(Device.Pins.GPIO13),
                new Led(Device.Pins.GPIO19),
                new Led(Device.Pins.GPIO26),

                //new Led(Device.Pins.GPIO14), // Not Available
                //new Led(Device.Pins.GPIO15), // Not Available
                new Led(Device.Pins.GPIO18),
                new Led(Device.Pins.GPIO23),
                new Led(Device.Pins.GPIO24),
                new Led(Device.Pins.GPIO25),
                //new Led(Device.Pins.GPIO8), // Throws Exception
                //new Led(Device.Pins.GPIO7), // Throws Exception 
                new Led(Device.Pins.GPIO12),
                new Led(Device.Pins.GPIO16),
                new Led(Device.Pins.GPIO20),
                new Led(Device.Pins.GPIO21),
            };

        return Task.CompletedTask;
    }

    public override async Task Run()
    {
        Resolver.Log.Info("Run...");

        while (true)
        {
            Resolver.Log.Error("Turning on each led every 100ms");
            for (int i = 0; i < leds.Count; i++)
            {
                leds[i].IsOn = true;
                await Task.Delay(100);
            }

            await Task.Delay(500);

            Resolver.Log.Info("Turning off each led every 100ms");
            for (int i = leds.Count - 1; i >= 0; i--)
            {
                leds[i].IsOn = false;
                await Task.Delay(100);
            }

            await Task.Delay(500);
        }
    }

    public static async Task Main(string[] args)
    {
        await MeadowOS.Start(args);
    }
}