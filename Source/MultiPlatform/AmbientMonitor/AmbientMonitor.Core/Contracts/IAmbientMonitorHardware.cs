using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors;

namespace AmbientMonitor.Core.Contracts;

public interface IAmbientMonitorHardware
{
    ITemperatureSensor? TemperatureSensor { get; }

    IPixelDisplay? Display { get; }

    RotationType DisplayRotation { get; }
}