using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Foundation.Sensors.Camera;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GridEYE;

// Change F7CoreComputeV2 to F7FeatherV2 for ProjectLab v2
public class MeadowApp : App<F7CoreComputeV2>
{
    private IProjectLabHardware _projectLab;
    private DisplayScreen _screen;
    private Amg8833 _camera;
    private Box[] _pixelBoxes;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        _projectLab = ProjectLab.Create();
        Resolver.Log.Info($"Running on ProjectLab Hardware {_projectLab.RevisionString}");

        _camera = new Amg8833(_projectLab.Qwiic.I2cBus);

        CreateLayout();
        return base.Initialize();
    }

    private void CreateLayout()
    {
        _pixelBoxes = new Box[64];
        _screen = new DisplayScreen(_projectLab.Display);
        var x = 0;
        var y = 0;
        var boxSize = 32;
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

    public override async Task Run()
    {
        Console.WriteLine("Run...");

        // raw data is faster and generates less garbage
        await DisplayWithRawData();

        // temerpatures is slower, but more intuitive
        // DisplayWithTemperatures()
    }

    private Task DisplayWithRawData()
    {
        Span<short> buffer = stackalloc short[64];

        while (true)
        {
            _camera.ReadRawPixels(buffer);

            _screen.BeginUpdate();

            for (var i = 0; i < buffer.Length; i++)
            {
                var color = buffer[i] switch
                {
                    < 20 * 4 => Color.Black,
                    < 22 * 4 => Color.DarkViolet,
                    < 24 * 4 => Color.DarkBlue,
                    < 26 * 4 => Color.DarkGreen,
                    < 28 * 4 => Color.DarkOrange,
                    < 30 * 4 => Color.Yellow,
                    _ => Color.White
                };

                _pixelBoxes[i].ForeColor = color;
            }

            _screen.EndUpdate();

            Thread.Sleep(100);
        }
    }

    private Task DisplayWithTemperatures()
    {
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

            Thread.Sleep(100);
        }
    }
}