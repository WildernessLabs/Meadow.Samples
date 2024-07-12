using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Distance;
using Meadow.Foundation.Servos;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Servos;
using Meadow.Units;
using System;
using System.Threading;
using System.Threading.Tasks;
using AU = Meadow.Units.Angle.UnitType;
using LU = Meadow.Units.Length.UnitType;

namespace ObstacleRadar;

// public class MeadowApp : App<F7FeatherV1> <- If you have a Meadow F7v1.*
public class MeadowApp : App<F7FeatherV2>
{
    private MicroGraphics graphics;
    private Vl53l0x sensor;
    private IAngularServo servo;
    private float[] radarData = new float[181];

    public override Task Initialize()
    {
        var onboardLed = new RgbPwmLed(
            redPwmPin: Device.Pins.OnboardLedRed,
            greenPwmPin: Device.Pins.OnboardLedGreen,
            bluePwmPin: Device.Pins.OnboardLedBlue);
        onboardLed.SetColor(Color.Red);

        var display = new St7789(
            spiBus: Device.CreateSpiBus(),
            chipSelectPin: Device.Pins.D02,
            dcPin: Device.Pins.D01,
            resetPin: Device.Pins.D00,
            width: 240, height: 240);

        graphics = new MicroGraphics(display);
        graphics.CurrentFont = new Font12x20();
        graphics.Rotation = RotationType._270Degrees;

        var i2cBus = Device.CreateI2cBus(I2cBusSpeed.FastPlus);
        sensor = new Vl53l0x(i2cBus);
        sensor.StartUpdating(TimeSpan.FromMilliseconds(200));

        servo = new Sg90(Device.Pins.D05);
        servo.Neutral();

        onboardLed.SetColor(Color.Green);

        return base.Initialize();
    }

    private void Draw()
    {
        int angle = 160;
        int increment = 4;
        int x, y = 0;

        while (true)
        {
            graphics.Clear();

            DrawRadar();

            graphics.DrawLine(120, 170, 105, (float)(angle * Math.PI / 180), Color.Yellow);

            if (angle >= 180) { increment = -4; }
            if (angle <= 0) { increment = 4; }

            angle += increment;
            servo.RotateTo(new Angle(angle, AU.Degrees));

            graphics.DrawText(0, 0, $"{180 - angle}°", Color.Yellow);

            if (sensor.Distance != null && sensor.Distance >= new Length(0, LU.Millimeters))
            {
                graphics.DrawText(170, 0, $"{sensor.Distance?.Millimeters}mm", Color.Yellow);
                radarData[angle] = (float)(sensor.Distance?.Millimeters / 2);
            }
            else
            {
                radarData[angle] = 0;
            }

            for (int i = 0; i < 180; i++)
            {
                x = 120 + (int)(radarData[i] * MathF.Cos(i * MathF.PI / 180f));
                y = 170 - (int)(radarData[i] * MathF.Sin(i * MathF.PI / 180f));
                graphics.DrawCircle(x, y, 2, Color.Yellow, true);
            }

            graphics.Show();
            Thread.Sleep(100);
        }
    }

    private void DrawRadar()
    {
        int xCenter = 120;
        int yCenter = 170;

        var radarColor = Color.LawnGreen;

        for (int i = 1; i < 5; i++)
        {
            graphics.DrawCircleQuadrant(xCenter, yCenter, 25 * i, 0, radarColor);
            graphics.DrawCircleQuadrant(xCenter, yCenter, 25 * i, 1, radarColor);
        }

        for (int i = 0; i < 7; i++)
        {
            graphics.DrawLine(xCenter, yCenter, 105, (float)(i * Math.PI / 6), radarColor);
        }
    }

    public override Task Run()
    {
        Draw();

        return base.Run();
    }
}