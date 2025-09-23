using Graphics.MicroGraphics.Dither;
using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Peripherals.Displays;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

public class MeadowApp : App<Desktop<SimulatedEpd5in65f>>
{
    static Image image;

    static IPixelBuffer ditheredBuffer;

    public static async Task Main(string[] args)
    {
        image = GetImageResource("gallery.bmp");

        var palette = new Color[]
        {
            Color.Black,
            Color.White,
            Color.Green,
            Color.Blue,
            Color.Red,
            Color.Yellow,
            Color.Orange
        };

        //dither for epaper 4-bit (7 color) display
        ditheredBuffer = PixelBufferDither.ToIndexed4(image.DisplayBuffer!, palette, DitherMode.FloydSteinberg, true);

        await MeadowOS.Start(args);
    }

    public override Task Run()
    {
        _ = Task.Run(() =>
        {
            Thread.Sleep(1000); // wait for the display to be ready

            var graphics = new MicroGraphics(Device.Display);

            var font = new Font12x16();

            graphics.Clear();
            graphics.DrawBuffer(0, 0, ditheredBuffer);
            graphics.DrawText(10, 10, "Simulated EPD 5.65\" F", Color.Black, ScaleFactor.X2, font: font);
            graphics.Show();
        });

        ExecutePlatformDisplayRunner();

        return Task.CompletedTask;
    }

    private void ExecutePlatformDisplayRunner()
    {
        if (Device.Display is SilkDisplay sd)
        {
            sd.Run();
        }
        else if (Device.Display is IVirtualPixelDisplay vd)
        {
            if (vd.Renderer is SilkDisplay s)
            {
                s.Run();
            }
        }

        MeadowOS.TerminateRun();
        System.Environment.Exit(0);
    }

    private static Image GetImageResource(string name)
    {
        var assemblyName = Assembly
             .GetExecutingAssembly()
             .GetName()
             .Name;

        var image = Image.LoadFromResource($"{assemblyName}.{name}");

        return image;
    }
}