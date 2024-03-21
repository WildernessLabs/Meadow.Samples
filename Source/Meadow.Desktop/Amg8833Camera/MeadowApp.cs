using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Foundation.Sensors.Camera;
using System.Threading.Tasks;

public class MeadowApp : App<Desktop>
{
    private FtdiExpander _ft232h;
    private DisplayScreen _screen;
    private Amg8833 _camera;
    private Box[] _pixelBoxes;

    public override Task Initialize()
    {
        Resolver.Log.Info("Creating Outputs");

        _ft232h = FtdiExpanderCollection.Devices[0];
        var bus = _ft232h.CreateI2cBus();
        _camera = new Amg8833(bus);

        CreateLayout();

        return base.Initialize();
    }

    private void CreateLayout()
    {
        _pixelBoxes = new Box[64];
        _screen = new DisplayScreen(Device.Display!);
        var x = 0;
        var y = 0;
        var boxSize = 40;
        for (var i = 0; i < _pixelBoxes.Length; i++)
        {
            _pixelBoxes[i] = new Box(x, y, boxSize, boxSize)
            {
                ForeColor = Color.Blue
            };

            _screen.Controls.Add(_pixelBoxes[i]);

            if (i % 8 == 7)
            {
                x = 0;
                y += boxSize;
            }
            else
            {
                x += boxSize;
            }
        }
    }

    public override Task Run()
    {
        // NOTE: this will not return until the display is closed
        ExecutePlatformDisplayRunner();

        _ = Task.Run(async () =>
        {
            await Task.Delay(1000);

            while (true)
            {
                var pixels = _camera.ReadPixels();

                _screen.BeginUpdate();

                for (var i = 0; i < pixels.Length; i++)
                {
                    var color = pixels[i].Celsius switch
                    {
                        < 20 => Color.Black,
                        < 22 => Color.DarkViolet,
                        < 24 => Color.DarkBlue,
                        < 26 => Color.DarkGreen,
                        < 28 => Color.DarkOrange,
                        < 30 => Color.Yellow,
                        _ => Color.White
                    };

                    _pixelBoxes[i].ForeColor = color;
                }

                _screen.EndUpdate();

                await Task.Delay(100);
            }
        });

        return Task.CompletedTask;
    }

    private void ExecutePlatformDisplayRunner()
    {
#if WINDOWS
        System.Windows.Forms.Application.Run(Device.Display as System.Windows.Forms.Form);
#endif
        if (Device.Display is GtkDisplay gtk)
        {
            gtk.Run();
        }
    }
}