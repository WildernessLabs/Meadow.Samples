using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Grove.Sensors.Moisture;
using Meadow.Peripherals.Leds;
using PlantMonitor.Controllers;
using System;
using System.Threading.Tasks;

namespace PlantMonitor;

// Change ProjectLabCoreComputeApp to ProjectLabFeatherApp for ProjectLab v2
public class MeadowApp : ProjectLabCoreComputeApp
{
    private IRgbPwmLed onboardLed;
    private MoistureSensor moistureSensor;
    private DisplayController displayController;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        Resolver.Log.Info($"Running on ProjectLab Hardware {Hardware.RevisionString}");

        onboardLed = Hardware.RgbLed;
        onboardLed.SetColor(Color.Red);

        displayController = DisplayController.Instance;
        displayController.Initialize(Hardware.Display);

        moistureSensor = new MoistureSensor(Hardware.IOTerminal.Pins.A1.CreateAnalogInputPort(1));
        var moistureSensorObserver = MoistureSensor.CreateObserver(
            handler: result =>
            {
                onboardLed.SetColor(Color.Purple);

                displayController.Update((int)ExtensionMethods.Map(result.New.Millivolts, 0, 1500, 0, 100));

                onboardLed.SetColor(Color.Green);
            },
            filter: null
        );
        moistureSensor.Subscribe(moistureSensorObserver);

        onboardLed.SetColor(Color.Green);

        return base.Initialize();
    }

    public override Task Run()
    {
        moistureSensor.StartUpdating(TimeSpan.FromMinutes(1));

        return base.Run();
    }
}