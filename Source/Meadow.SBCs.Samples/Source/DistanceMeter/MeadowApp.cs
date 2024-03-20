using Meadow;
using Meadow.Foundation.Sensors.Distance;
using Meadow.Units;

namespace DistanceMeter;

public class MeadowApp : App<RaspberryPi>
{
    private Vl53l0x sensor;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initializing hardware...");

        var i2cBus = Device.CreateI2cBus(1);
        sensor = new Vl53l0x(i2cBus);
        sensor.Updated += Sensor_Updated;

        return Task.CompletedTask;
    }

    public override Task Run()
    {
        sensor.StartUpdating(TimeSpan.FromMilliseconds(250));

        return Task.CompletedTask;
    }

    private void Sensor_Updated(object sender, IChangeResult<Length> result)
    {
        if (result.New == null) { return; }

        if (result.New < new Length(0, Length.UnitType.Millimeters))
        {
            Resolver.Log.Info("out of range.");
        }
        else
        {
            Resolver.Log.Info($"{result.New.Millimeters}mm / {result.New.Inches:n3}\"");
        }
    }

    private static async Task Main(string[] args)
    {
        await MeadowOS.Start(args);
    }
}