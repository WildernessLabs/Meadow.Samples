using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Motion;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using SimpleJpegDecoder;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace MotionDetector;

// public class MeadowApp : App<F7FeatherV1> <- If you have a Meadow F7v1.*
public class MeadowApp : App<F7FeatherV2>
{
    MicroGraphics graphics;
    ParallaxPir motionSensor;

    public override Task Initialize()
    {
        var onboardLed = new RgbPwmLed(
            redPwmPin: Device.Pins.OnboardLedRed,
            greenPwmPin: Device.Pins.OnboardLedGreen,
            bluePwmPin: Device.Pins.OnboardLedBlue);
        onboardLed.SetColor(Color.Red);

        var display = new St7789
        (
            spiBus: Device.CreateSpiBus(),
            chipSelectPin: null,
            dcPin: Device.Pins.D01,
            resetPin: Device.Pins.D00,
            width: 240, height: 240
        );

        graphics = new MicroGraphics(display);
        graphics.Rotation = RotationType._270Degrees;

        motionSensor = new ParallaxPir(Device.Pins.D08, InterruptMode.EdgeFalling, ResistorMode.Disabled, TimeSpan.FromMilliseconds(5), TimeSpan.FromMilliseconds(0));
        motionSensor.OnMotionStart += MotionSensorMotionStart;
        motionSensor.OnMotionEnd += MotionSensorMotionEnd;

        onboardLed.SetColor(Color.Green);

        return base.Initialize();
    }

    void LoadScreen()
    {
        Console.WriteLine("LoadScreen...");

        graphics.Clear(true);

        graphics.Stroke = 1;
        graphics.DrawRectangle(
            x: 0,
            y: 0,
            width: graphics.Width,
            height: graphics.Height,
            color: Color.White);
        graphics.DrawRectangle(
            x: 5,
            y: 5,
            width: graphics.Width - 10,
            height: graphics.Height - 10,
            color: Color.White);

        graphics.DrawCircle(
            centerX: graphics.Width / 2,
            centerY: graphics.Height / 2,
            radius: (graphics.Width / 2) - 10,
            color: Color.FromHex("#EF7D3B"),
            filled: true);

        DisplayJPG(55, 40);

        graphics.CurrentFont = new Font8x12();

        string textMotion = "MOTION";
        graphics.DrawText(
            (graphics.Width - textMotion.Length * 16) / 2,
            140,
            textMotion,
            Color.Black,
            ScaleFactor.X2);

        string textDetector = "DETECTOR";
        graphics.DrawText(
            (graphics.Width - textDetector.Length * 16) / 2,
            165,
            textDetector,
            Color.Black,
            ScaleFactor.X2);

        graphics.Show();
    }

    void DisplayJPG(int x, int y)
    {
        var jpgData = LoadResource("meadow.jpg");
        var decoder = new JpegDecoder();
        var jpg = decoder.DecodeJpeg(jpgData);

        int imageX = 0;
        int imageY = 0;
        byte r, g, b;

        for (int i = 0; i < jpg.Length; i += 3)
        {
            r = jpg[i];
            g = jpg[i + 1];
            b = jpg[i + 2];

            graphics.DrawPixel(imageX + x, imageY + y, Color.FromRgb(r, g, b));

            imageX++;
            if (imageX % decoder.Width == 0)
            {
                imageY++;
                imageX = 0;
            }
        }

        graphics.Show();
    }

    byte[] LoadResource(string filename)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = $"MotionDetector.{filename}";

        using (Stream stream = assembly.GetManifestResourceStream(resourceName))
        {
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }

    private void MotionSensorMotionEnd(object sender)
    {
        Console.WriteLine("End");
        //onboardLed.SetColor(Color.Cyan);
    }

    private void MotionSensorMotionStart(object sender)
    {
        Console.WriteLine("Start");
        //displayonboardLed.SetColor(Color.Magenta);
        Thread.Sleep(1000);
    }

    public override Task Run()
    {
        LoadScreen();

        return base.Run();
    }
}