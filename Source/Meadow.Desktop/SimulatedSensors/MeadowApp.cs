using Meadow;
using Meadow.Foundation.Sensors;
using Meadow.Foundation.Sensors.Hid;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Light;
using Meadow.Units;
using System.Threading.Tasks;

namespace SimulatedSensors;

public class MeadowApp : App<Desktop>
{
    private bool UseKeyboardForValueChanges { get; set; } = false;
    private bool UseRandomValueForAutomaticChanges { get; set; } = false;

    public override Task Initialize()
    {
        Resolver.Log.Info("Creating Sensors");

        var lightSensor = UseKeyboardForValueChanges
            ? CreateKeyboardDrivenSensor()
            : CreateAutoChangingValueSensor();

        lightSensor.Updated += OnLightSensorUpdated;
        return Task.CompletedTask;
    }

    private void OnLightSensorUpdated(object? sender, IChangeResult<Illuminance> e)
    {
        Resolver.Log.Info($"Light level now: {e.New.Lux} lux");
    }

    private ILightSensor CreateKeyboardDrivenSensor()
    {
        var keyboard = new Keyboard();
        var up = keyboard.Pins.Up.CreateDigitalInterruptPort(Meadow.Hardware.InterruptMode.EdgeFalling);
        var down = keyboard.Pins.Down.CreateDigitalInterruptPort(Meadow.Hardware.InterruptMode.EdgeFalling);

        return new SimulatedLightSensor(new Illuminance(100), Illuminance.Zero, new Illuminance(1000), up, down);
    }

    private ILightSensor CreateAutoChangingValueSensor()
    {
        return new SimulatedLightSensor(
            new Illuminance(100), Illuminance.Zero, new Illuminance(1000),
            UseRandomValueForAutomaticChanges ? SimulationBehavior.RandomWalk : SimulationBehavior.Sawtooth);
    }
}