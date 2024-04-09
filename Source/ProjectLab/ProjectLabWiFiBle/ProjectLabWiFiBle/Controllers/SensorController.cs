using Meadow;
using Meadow.Devices;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Atmospheric;
using Meadow.Peripherals.Sensors.Light;
using Meadow.Peripherals.Sensors.Motion;
using MeadowConnectedSample.Models;
using System;
using System.Threading.Tasks;

namespace MeadowConnectedSample.Controllers;

public class SensorController
{
    public event EventHandler<AtmosphericConditions> AtmosphericConditionsChanged = default!;
    public event EventHandler<LightConditions> LightConditionsChanged = default!;
    public event EventHandler<MotionConditions> MotionConditionsChanged = default!;

    private ITemperatureSensor temperatureSensor;
    private IBarometricPressureSensor pressureSensor;
    private IHumiditySensor humiditySensor;
    private ILightSensor lightSensor;
    private IGyroscope gyroscope;
    private IAccelerometer accelerometer;

    public SensorController(IProjectLabHardware hardware)
    {
        temperatureSensor = hardware.TemperatureSensor;
        pressureSensor = hardware.BarometricPressureSensor;
        humiditySensor = hardware.HumiditySensor;
        lightSensor = hardware.LightSensor;
        gyroscope = hardware.Gyroscope;
        accelerometer = hardware.Accelerometer;
    }

    public async Task StartUpdating(TimeSpan updateInterval)
    {
        while (true)
        {
            var temperature = await temperatureSensor.Read();
            var pressure = await pressureSensor.Read();
            var humidity = await humiditySensor.Read();
            var illuminance = await lightSensor.Read();
            var angularVelocityReading = await gyroscope.Read();
            var acceleration3DReading = await accelerometer.Read();

            Resolver.Log.Info($"" +
                $"Temperature: {temperature.Celsius:N1} | " +
                $"Pressure: {pressure.StandardAtmosphere:N1} | " +
                $"Humidity: {humidity.Percent:N1} | " +
                $"Illuminance: {illuminance.Lux:N1} | " +
                $"Acceleration3D: ({acceleration3DReading.X.CentimetersPerSecondSquared:N1}, {acceleration3DReading.Y.CentimetersPerSecondSquared:N1}, {acceleration3DReading.Z.CentimetersPerSecondSquared:N1}) | " +
                $"AngularVelocity3D: ({angularVelocityReading.X.RevolutionsPerMinute:N1},{angularVelocityReading.Y.RevolutionsPerMinute:N1},{angularVelocityReading.Z.RevolutionsPerMinute:N1}) ");

            var atmosphericConditions = new AtmosphericConditions()
            {
                Temperature = temperature,
                Pressure = pressure,
                Humidity = humidity
            };
            AtmosphericConditionsChanged?.Invoke(this, atmosphericConditions);

            var lightConditions = new LightConditions()
            {
                Illuminance = illuminance
            };
            LightConditionsChanged?.Invoke(this, lightConditions);

            var motionConditions = new MotionConditions()
            {
                Acceleration3D = acceleration3DReading,
                AngularVelocity3D = angularVelocityReading
            };
            MotionConditionsChanged?.Invoke(this, motionConditions);

            await Task.Delay(updateInterval);
        }
    }
}