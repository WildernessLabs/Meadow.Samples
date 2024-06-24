using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Sensors.Camera;
using Meadow.Peripherals.Leds;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ThermalCamera;

// Change ProjectLabCoreComputeApp to ProjectLabFeatherApp for ProjectLab v2
public class MeadowApp : ProjectLabCoreComputeApp
{
    private IRgbPwmLed onboardLed;
    private Mlx90640 thermalCamera;
    private MicroGraphics graphics;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        Resolver.Log.Info($"Running on ProjectLab Hardware {Hardware.RevisionString}");

        onboardLed = Hardware.RgbLed;
        onboardLed.SetColor(Color.Red);

        graphics = new MicroGraphics(Hardware.Display);

        thermalCamera = new Mlx90640(Hardware.Qwiic.I2cBus);

        onboardLed.SetColor(Color.Green);

        return base.Initialize();
    }

    public override Task Run()
    {
        Console.WriteLine("Run...");

        float[] frame;

        //define a scaled pixel size that fills the 240x240 display
        int pixelW = 7;
        int pixelH = 10;

        Color pixelColor;

        while (true)
        {
            //get the raw thermal data from the camera
            frame = thermalCamera.ReadRawData();

            byte pixelValue;

            graphics.Clear();

            //loop over every data point from the thermal camera (resolution is 32x24)
            for (byte height = 0; height < 24; height++)
            {
                for (byte width = 0; width < 32; width++)
                {
                    pixelValue = (byte)((byte)frame[height * 32 + width] << 0);

                    //calculate a color based on the pixel value, using read and green to get shades of yellow
                    pixelColor = new Color(pixelValue, pixelValue, 0);
                    graphics.DrawRectangle(8 + width * pixelW, height * pixelH, pixelW, pixelH, pixelColor, true);
                }
            }

            graphics.Show();

            Thread.Sleep(100);
        }
    }
}