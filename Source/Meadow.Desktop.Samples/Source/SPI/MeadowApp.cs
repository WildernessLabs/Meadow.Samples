using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Peripherals.Displays;

public class MeadowApp : App<Windows>
{
    private FtdiExpander _expander = FtdiExpanderCollection.Devices[0];
    private St7789 _display;

    public static async Task Main(string[] args)
    {
        await MeadowOS.Start(args);
    }

    public override Task Initialize()
    {
        Console.WriteLine("Creating SPI Bus");

        var bus = _expander.CreateSpiBus();

        Console.WriteLine("Creating Display");

        _display = new St7789(
                    spiBus: bus,
                    chipSelectPin: _expander.Pins.C0,
                    dcPin: _expander.Pins.C1,
                    resetPin: _expander.Pins.C2,
                    width: 240, height: 240,
                    colorMode: ColorMode.Format16bppRgb565);

        return base.Initialize();
    }

    public override async Task Run()
    {
        while (true)
        {
            _display.Fill(Color.Red, true);
            await Task.Delay(1000);
            _display.Fill(Color.Green, true);
            await Task.Delay(1000);
            _display.Fill(Color.Blue, true);
            await Task.Delay(1000);
        }
    }
}