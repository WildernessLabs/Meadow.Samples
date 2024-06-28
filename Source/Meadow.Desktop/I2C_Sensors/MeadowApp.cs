using Meadow;
using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Foundation.Sensors.Motion;
using Meadow.Units;
using System;
using System.Threading.Tasks;

namespace I2C_Sensors;

public class MeadowApp : App<Desktop>
{
    private Mpu6050 motionSensor;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        if (FtdiExpanderCollection.Devices.Count == 0)
        {
            Resolver.Log.Info($"No expanders found!");
            return Task.CompletedTask;
        }

        Resolver.Log.Info($"{FtdiExpanderCollection.Devices.Count} expanders found");

        var expander = FtdiExpanderCollection.Devices[0];

        Resolver.Log.Info("Sensor initializing...");

        motionSensor = new Mpu6050(expander.CreateI2cBus());
        motionSensor.Updated += mpu6050_Updated;

        Resolver.Log.Info("Sensor initialized");

        return Task.CompletedTask;
    }

    private void mpu6050_Updated(object? sender, IChangeResult<(Acceleration3D? Acceleration3D, AngularVelocity3D? AngularVelocity3D, Temperature? Temperature)> e)
    {
        Resolver.Log.Info($"Acceleration 3D: ({e.New.Acceleration3D.Value.X.CentimetersPerSecondSquared:N2}, {e.New.Acceleration3D.Value.Y.CentimetersPerSecondSquared:N2}, {e.New.Acceleration3D.Value.Z.CentimetersPerSecondSquared:N2}) |" +
                          $"Angular Velocity 3D: ({e.New.AngularVelocity3D.Value.X.DegreesPerSecond:N2}, {e.New.AngularVelocity3D.Value.Y.DegreesPerSecond:N2}, {e.New.AngularVelocity3D.Value.Z.DegreesPerSecond:N2}) |" +
                          $"Temperature: {e.New.Temperature.Value.Celsius:N2}°C");
    }

    public override Task Run()
    {
        Resolver.Log.Info("Run...");

        motionSensor.StartUpdating(TimeSpan.FromSeconds(5));

        return Task.CompletedTask;
    }
}