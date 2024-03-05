using Meadow;
using Meadow.Foundation.Leds;
using Meadow.Peripherals.Leds;
using System;
using System.Threading.Tasks;

public class MeadowApp : LinuxApp<RaspberryPi>
{
    RgbLed rgbLed;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        rgbLed = new RgbLed(
            Device.Pins.GPIO16,
            Device.Pins.GPIO20,
            Device.Pins.GPIO21);

        return base.Initialize();
    }

    public override async Task Run()
    {
        Resolver.Log.Info("Run...");

        while (true)
        {
            Resolver.Log.Info("Going through each color...");
            for (int i = 0; i < (int)RgbLedColors.count; i++)
            {
                rgbLed.SetColor((RgbLedColors)i);
                await Task.Delay(500);
            }

            await Task.Delay(1000);

            Resolver.Log.Info("Blinking through each color (on 500ms / off 500ms)...");
            for (int i = 0; i < (int)RgbLedColors.count; i++)
            {
                await rgbLed.StartBlink((RgbLedColors)i);
                await Task.Delay(3000);
                await rgbLed.StopAnimation();
                rgbLed.IsOn = false;
            }

            await Task.Delay(1000);

            Resolver.Log.Info("Blinking through each color (on 1s / off 1s)...");
            for (int i = 0; i < (int)RgbLedColors.count; i++)
            {
                await rgbLed.StartBlink((RgbLedColors)i, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
                await Task.Delay(3000);
                await rgbLed.StopAnimation();
                rgbLed.IsOn = false;
            }

            await Task.Delay(1000);
        }
    }

    static async Task Main(string[] args)
    {
        await MeadowOS.Start(args);
    }
}