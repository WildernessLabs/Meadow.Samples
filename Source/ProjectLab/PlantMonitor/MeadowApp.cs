using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Grove.Sensors.Moisture;
using Meadow.Peripherals.Leds;
using PlantMonitor.Controllers;
using System;
using System.Threading.Tasks;

namespace PlantMonitor;

// Change F7CoreComputeV2 to F7FeatherV2 for ProjectLab v2
public class MeadowApp : App<F7CoreComputeV2>
{
    private IRgbPwmLed onboardLed;
    private IProjectLabHardware projectLab;
    private MoistureSensor moistureSensor;
    private DisplayController displayController;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        projectLab = ProjectLab.Create();
        Resolver.Log.Info($"Running on ProjectLab Hardware {projectLab.RevisionString}");

        onboardLed = projectLab.RgbLed;
        onboardLed.SetColor(Color.Red);

        displayController = DisplayController.Instance;
        displayController.Initialize(projectLab.Display);

        moistureSensor = new MoistureSensor(Device.Pins.A01.CreateAnalogInputPort(1));
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