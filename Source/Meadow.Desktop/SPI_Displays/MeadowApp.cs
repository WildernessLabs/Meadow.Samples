using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Foundation.ICs.IOExpanders;
using System.Threading.Tasks;

namespace SPI_Displays;

public class MeadowApp : App<Desktop>
{
    //private St7789 display;
    private Ili9341 display;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        var expander = FtdiExpanderCollection.Devices[0];

        Resolver.Log.Info("Creating Display");

        //display = new St7789(
        //            spiBus: bus,
        //            chipSelectPin: expander.Pins.C0,
        //            dcPin: expander.Pins.C1,
        //            resetPin: expander.Pins.C2,
        //            width: 240, height: 240,
        //            colorMode: ColorMode.Format16bppRgb565);

        display = new Ili9341(
            expander.CreateSpiBus(),
            chipSelectPin: expander.Pins.C0,
            dcPin: expander.Pins.C2,
            resetPin: expander.Pins.C1,
            width: 320,
            height: 240);

        return base.Initialize();
    }

    public override async Task Run()
    {
        Resolver.Log.Info("Run...");

        while (true)
        {
            display.Fill(Color.Red, true);
            await Task.Delay(1000);
            display.Fill(Color.Green, true);
            await Task.Delay(1000);
            display.Fill(Color.Blue, true);
            await Task.Delay(1000);
        }
    }
}