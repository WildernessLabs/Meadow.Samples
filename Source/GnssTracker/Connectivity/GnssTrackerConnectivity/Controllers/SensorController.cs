using GnssTrackerConnectivity.Models;
using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Atmospheric;
using Meadow.Peripherals.Sensors.Environmental;
using Meadow.Peripherals.Sensors.Location.Gnss;
using Meadow.Peripherals.Sensors.Motion;
using System;
using System.Threading.Tasks;

namespace GnssTrackerConnectivity.Controllers;

public class SensorController
{
    private ITemperatureSensor temperatureSensor;
    private IBarometricPressureSensor pressureSensor;
    private IHumiditySensor humiditySensor;
    private IGasResistanceSensor gasResistanceSensor;
    private ICO2ConcentrationSensor co2ConcentrationSensor;
    private IGnssSensor gnssSensor;
    private IGyroscope gyroscope;
    private IAccelerometer accelerometer;
    private IAnalogInputPort batteryVoltageInput;
    private IAnalogInputPort solarVoltageInput;

    public AtmosphericConditions AtmosphericConditions { get; set; }
    public MotionConditions MotionConditions { get; set; }

    public event EventHandler<AtmosphericConditions> AtmosphericConditionsChanged = default!;
    public event EventHandler<MotionConditions> MotionConditionsChanged = default!;

    public SensorController(IGnssTrackerHardware hardware)
    {
        temperatureSensor = hardware.TemperatureSensor;
        pressureSensor = hardware.BarometricPressureSensor;
        humiditySensor = hardware.HumiditySensor;
        gasResistanceSensor = hardware.GasResistanceSensor;
        co2ConcentrationSensor = hardware.CO2ConcentrationSensor;
        gnssSensor = hardware.Gnss;
        gyroscope = hardware.Gyroscope;
        accelerometer = hardware.Accelerometer;
        batteryVoltageInput = hardware.BatteryVoltageInput;
        solarVoltageInput = hardware.SolarVoltageInput;

        Resolver.Services.Add(this);
    }

    public async Task StartUpdating(TimeSpan updateInterval)
    {
        while (true)
        {
            var temperature = await temperatureSensor.Read();
            var pressure = await pressureSensor.Read();
            var humidity = await humiditySensor.Read();
            var gasResistance = await gasResistanceSensor.Read();
            var co2Concentration = await co2ConcentrationSensor.Read();
            var angularVelocityReading = await gyroscope.Read();
            var acceleration3DReading = await accelerometer.Read();
            var batteryVoltageReading = await batteryVoltageInput.Read();
            var solarVoltageReading = await solarVoltageInput.Read();

            Resolver.Log.Info($"" +
                $"Temperature: {temperature.Celsius:N1} | " +
                $"Pressure: {pressure.StandardAtmosphere:N1} | " +
                $"Humidity: {humidity.Percent:N1} | " +
                $"Gas Resistance: {gasResistance.Megaohms:N1} | " +
                $"CO2 Concentration: {co2Concentration.PartsPerMillion:N1} | " +
                $"AngularVelocity3D: ({angularVelocityReading.X.RevolutionsPerMinute:N1},{angularVelocityReading.Y.RevolutionsPerMinute:N1},{angularVelocityReading.Z.RevolutionsPerMinute:N1}) | " +
                $"Acceleration3D: ({acceleration3DReading.X.CentimetersPerSecondSquared:N1}, {acceleration3DReading.Y.CentimetersPerSecondSquared:N1}, {acceleration3DReading.Z.CentimetersPerSecondSquared:N1}) | " +
                $"Battery Voltage: {batteryVoltageReading.Volts:N1} | " +
                $"Solar Voltage: {solarVoltageReading.Volts:N1} ");

            AtmosphericConditions = new AtmosphericConditions()
            {
                Temperature = temperature,
                Pressure = pressure,
                Humidity = humidity,
                GasResistance = gasResistance,
                Co2Concentration = co2Concentration
            };
            AtmosphericConditionsChanged?.Invoke(this, AtmosphericConditions);

            MotionConditions = new MotionConditions()
            {
                Acceleration3D = acceleration3DReading,
                AngularVelocity3D = angularVelocityReading
            };
            MotionConditionsChanged?.Invoke(this, MotionConditions);

            await Task.Delay(updateInterval);
        }
    }
}