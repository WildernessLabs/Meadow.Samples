using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Sensors.Hid;
using Meadow.Hardware;
using Meadow.Units;
using System;
using System.Threading.Tasks;

namespace Touchscreen_Demo;

// Change ProjectLabCoreComputeApp to ProjectLabFeatherApp for ProjectLab v2
public class MeadowApp : ProjectLabCoreComputeApp
{
    public enum MikrobusPort
    {
        Port1,
        Port2
    }

    // change this porperty to change the port to test
    public MikrobusPort TouchPortToTest { get; } = MikrobusPort.Port1;
    public bool DrawToDisplay { get; set; } = true;

    public override async Task Run()
    {
        Console.WriteLine("Creating ProjLab...");

        var connector = TouchPortToTest switch
        {
            MikrobusPort.Port1 => Hardware.MikroBus1,
            _ => Hardware.MikroBus2,
        };

        connector.SpiBus.Configuration.Speed = new Frequency(100, Frequency.UnitType.Kilohertz);
        var touchscreen = new Xpt2046(
            connector.SpiBus,
            connector.Pins.INT.CreateDigitalInterruptPort(InterruptMode.EdgeFalling, ResistorMode.Disabled),
            connector.Pins.CS.CreateDigitalOutputPort(true));

        touchscreen.TouchDown += (s, p) => Resolver.Log.Info($"Touch down at ({p.RawX}, {p.RawY})");
        touchscreen.TouchUp += (s, p) => Resolver.Log.Info($"Touch up");
        touchscreen.TouchMoved += (s, p) => Resolver.Log.Info($"Touch at ({p.RawX}, {p.RawY})");

        if (DrawToDisplay)
        {
            var graphics = new MicroGraphics(Hardware.Display)
            {
                Rotation = Meadow.Peripherals.Displays.RotationType._270Degrees
            };

            var random = new Random();

            while (true)
            {
                graphics.DrawRectangle(10, 10, Hardware.Display.Width - 20, Hardware.Display.Height - 20,
                    Color.FromRgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255)),
                    true);
                graphics.Show();
                await Task.Delay(500);
            }
        }
    }

    /*
        private CircleDemo _demo;

        public override async Task Initialize()
        {
            var display = new DisplayScreen(
                Hardware.Display,
                touchScreen: Hardware.Touchscreen,
                rotation: Meadow.Peripherals.Displays.RotationType._270Degrees);

            if (Hardware.Touchscreen == null)
            {
                Resolver.Log.Error($"This demo requires a touchscreen, which is available of version hardware revision 3.f or later");
                return;
            }

            // retrieve stored calibration
            var ts = new TouchscreenCalibrationService(display);

            //        ts.EraseCalibrationData();

            var calData = ts.GetSavedCalibrationData();
            if (calData != null)
            {
                (Hardware.Touchscreen as ICalibratableTouchscreen).SetCalibrationData(calData);
            }
            else
            {
                Resolver.Log.Info("Calibrating");

                await ts.Calibrate(true);
            }

            _demo = new CircleDemo(display, Hardware.Touchscreen);
        }

        public override Task Run()
        {
            _demo.Start();
            return base.Run();
        }
    */
}