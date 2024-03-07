using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Hardware;
using System.Threading.Tasks;

namespace Touchscreen_Demo;

public class MeadowApp : App<F7CoreComputeV2>
{
    private CircleDemo _demo;

    public override async Task Initialize()
    {
        var projectLab = ProjectLab.Create();
        var display = new DisplayScreen(
            projectLab.Display,
            touchScreen: projectLab.Touchscreen,
            rotation: Meadow.Peripherals.Displays.RotationType._270Degrees);

        if (projectLab.Touchscreen == null)
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
            (projectLab.Touchscreen as ICalibratableTouchscreen).SetCalibrationData(calData);
        }
        else
        {
            Resolver.Log.Info("Calibrating");

            await ts.Calibrate(true);
        }

        _demo = new CircleDemo(display, projectLab.Touchscreen);
    }

    public override Task Run()
    {
        _demo.Start();
        return base.Run();
    }
}