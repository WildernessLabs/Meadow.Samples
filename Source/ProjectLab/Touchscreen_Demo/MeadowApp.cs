using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Hardware;
using System.Threading.Tasks;

namespace Touchscreen_Demo;

// Change ProjectLabCoreComputeApp to ProjectLabFeatherApp for ProjectLab v2
public class MeadowApp : ProjectLabCoreComputeApp
{
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
}