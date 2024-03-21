using Meadow;
using Meadow.Foundation.Displays.Lcd;
using Meadow.Foundation.ICs.IOExpanders;
using System;
using System.Threading;
using System.Threading.Tasks;

public class MeadowApp : App<Desktop>
{
    private CharacterDisplay? display;

    public override Task Initialize()
    {
        Resolver.Log.Info("Creating Outputs");

        var expander = FtdiExpanderCollection.Devices[0];

        display = new CharacterDisplay
            (
                pinRS: expander.Pins.C5,
                pinE: expander.Pins.C4,
                pinD4: expander.Pins.C3,
                pinD5: expander.Pins.C2,
                pinD6: expander.Pins.C1,
                pinD7: expander.Pins.C0,
                rows: 4, columns: 20
            );

        return Task.CompletedTask;
    }

    void UpdateCountdown()
    {
        var today = DateTime.Now;

        display?.WriteLine($"{today.ToString("MMMM dd, yyyy")}", 2);
        display?.WriteLine($"{today.ToString("hh:mm:ss tt")}", 3);
    }

    public override Task Run()
    {
        display?.WriteLine($"Wilderness Labs", 0);
        display?.WriteLine($"Meadow.Windows ", 1);

        while (true)
        {
            UpdateCountdown();
            Thread.Sleep(1000);
        }
    }

    public static async Task Main(string[] args)
    {
        await MeadowOS.Start(args);
    }
}