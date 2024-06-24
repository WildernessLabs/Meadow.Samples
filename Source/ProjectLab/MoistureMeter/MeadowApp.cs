using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Grove.Sensors.Moisture;
using Meadow.Peripherals.Leds;
using MoistureMeter.Controllers;
using System;
using System.Threading.Tasks;

namespace MoistureMeter;

// Change ProjectLabCoreComputeApp to ProjectLabFeatherApp for ProjectLab v2
public class MeadowApp : ProjectLabCoreComputeApp
{
    private IRgbPwmLed onboardLed;
    private MoistureSensor sensor;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        Resolver.Log.Info($"Running on ProjectLab Hardware {Hardware.RevisionString}");

        onboardLed = Hardware.RgbLed;
        onboardLed.SetColor(Color.Red);

        DisplayController.Instance.Initialize(Hardware.Display);

        sensor = new MoistureSensor(Hardware.GroveAnalog.Pins.D0);

        sensor.Updated += (sender, result) =>
        {
            var percentage = (int)ExtensionMethods.Map(result.New.Millivolts, 0, 1750, 0, 100);

            DisplayController.Instance.UpdatePercentage(Math.Clamp(percentage, 0, 100));
        };

        sensor.StartUpdating(TimeSpan.FromMilliseconds(1000));

        onboardLed.SetColor(Color.Green);

        return base.Initialize();
    }
}