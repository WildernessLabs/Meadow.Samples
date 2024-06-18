using AmbientMonitor.Core.Contracts;
using Meadow.Devices;
using Meadow.Foundation.Sensors;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors;
using Meadow.Units;

namespace AmbientMonitor.F7;

internal class AmbientMonitorF7FeatherHardware : IAmbientMonitorHardware
{
    private readonly ITemperatureSensor temperatureSensor;

    public IPixelDisplay? Display => null;

    public RotationType DisplayRotation => RotationType.Default;

    public ITemperatureSensor? TemperatureSensor => temperatureSensor;

    public AmbientMonitorF7FeatherHardware(F7FeatherBase device)
    {
        temperatureSensor = new SimulatedTemperatureSensor(
            22.Celsius(), 20.Celsius(), 24.Celsius());
    }
}