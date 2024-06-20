using AmbientMonitor.Core.Contracts;
using AmbientMonitor.Core.Models;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Atmospheric;
using System;
using System.Threading.Tasks;

namespace AmbientMonitor.Core.Controllers;

public class SensorController
{
    private TimeSpan SAMPLE_INTERVAL = TimeSpan.FromSeconds(15);

    private ITemperatureSensor? temperatureSensor;
    private IBarometricPressureSensor? barometricPressureSensor;
    private IHumiditySensor? humiditySensor;

    public AtmosphericConditions AtmosphericConditions { get; set; }

    public event EventHandler<AtmosphericConditions> Updated = default!;

    public SensorController(IAmbientMonitorHardware hardware)
    {
        temperatureSensor = hardware.TemperatureSensor;
        barometricPressureSensor = hardware.BarometricPressureSensor;
        humiditySensor = hardware.HumiditySensor;
    }

    public async Task StartUpdating(TimeSpan updateInterval)
    {
        while (true)
        {
            var temperature = await temperatureSensor.Read();
            var pressure = await barometricPressureSensor.Read();
            var humidity = await humiditySensor.Read();

            AtmosphericConditions = new AtmosphericConditions()
            {
                Temperature = temperature,
                Pressure = pressure,
                Humidity = humidity
            };
            Updated?.Invoke(this, AtmosphericConditions);

            await Task.Delay(updateInterval);
        }
    }
}