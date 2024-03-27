using Meadow;
using Meadow.Foundation.Displays.Lcd;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CharacterDisplaySample;

public class MeadowApp : App<RaspberryPi>
{
    private CharacterDisplay? display;

    public override Task Initialize()
    {
        Resolver.Log.Info("Creating Outputs");

        display = new CharacterDisplay
            (
                pinRS: Device.Pins.GPIO21,
                pinE: Device.Pins.GPIO20,
                pinD4: Device.Pins.GPIO16,
                pinD5: Device.Pins.GPIO12,
                pinD6: Device.Pins.GPIO23,
                pinD7: Device.Pins.GPIO18,
                rows: 4, columns: 20
            );

        display.Write("Hello Meadow!");

        return Task.CompletedTask;
    }

    private void UpdateCountdown()
    {
        var today = DateTime.Now;

        display?.WriteLine($"{today.ToString("MMMM dd, yyyy")}", 2);
        display?.WriteLine($"{today.ToString("hh:mm:ss tt")}", 3);
    }

    public override Task Run()
    {
        display?.WriteLine($"Wilderness Labs", 0);
        display?.WriteLine($"Meadow.Linux ", 1);

        while (true)
        {
            UpdateCountdown();
            Thread.Sleep(1000);
        }
    }
}