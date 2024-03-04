using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.ICs.IOExpanders;
using System.Threading.Tasks;

public class MeadowApp : App<Desktop>
{
    private FtdiExpander _expander = FtdiExpanderCollection.Devices[0];
    private Max7219 _display;
    private MicroGraphics _graphics;

    public static async Task Main(string[] args)
    {
        await MeadowOS.Start(args);
    }

    public override Task Initialize()
    {
        Resolver.Log.Info("Creating SPI Bus");

        var bus = _expander.CreateSpiBus();

        Resolver.Log.Info("Creating Display");

        _display = new Max7219(bus, _expander.Pins.C0, 4, 3, Max7219.Max7219Mode.Display);
        _graphics = new MicroGraphics(_display);

        return base.Initialize();
    }

    public override async Task Run()
    {
        //await Blink();
        await Alternate();
    }

    private async Task Blink()
    {
        while (true)
        {
            _graphics.DrawRectangle(0, 0, _graphics.Width, _graphics.Height, Color.White, true);
            _graphics.Show();
            await Task.Delay(1000);
            _graphics.DrawRectangle(0, 0, _graphics.Width, _graphics.Height, Color.Black, true);
            _graphics.Show();
            await Task.Delay(1000);
        }
    }

    private async Task Alternate()
    {
        while (true)
        {
            _graphics.Clear();
            _graphics.DrawRectangle(0, 0, _graphics.Width / 2, _graphics.Height, Color.White, true);
            _graphics.Show();
            await Task.Delay(1000);
            _graphics.Clear();
            _graphics.DrawRectangle(_graphics.Width / 2, 0, _graphics.Width / 2, _graphics.Height, Color.White, true);
            _graphics.Show();
            await Task.Delay(1000);
        }
    }
}
