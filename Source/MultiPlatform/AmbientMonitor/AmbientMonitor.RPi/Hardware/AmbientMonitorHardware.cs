using AmbientMonitor.Core.Contracts;
using Meadow;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors;

namespace AmbientMonitor.RPi.Hardware;

internal class AmbientMonitorHardware : IAmbientMonitorHardware
{
    private readonly RaspberryPi device;
    private readonly IPixelDisplay? display = null;
    private readonly ITemperatureSensor temperatureSimulator;

    public IPixelDisplay? Display => display;

    public RotationType DisplayRotation => RotationType.Default;

    public ITemperatureSensor? TemperatureSensor => temperatureSimulator;

    public AmbientMonitorHardware(RaspberryPi device)
    {
        this.device = device;
    }
}