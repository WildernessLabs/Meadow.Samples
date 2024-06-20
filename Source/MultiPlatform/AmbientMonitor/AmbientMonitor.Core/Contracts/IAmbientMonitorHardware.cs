using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Atmospheric;
using Meadow.Peripherals.Sensors.Buttons;

namespace AmbientMonitor.Core.Contracts;

public interface IAmbientMonitorHardware
{
    IButton? LeftButton { get; }

    IButton? RightButton { get; }

    IPixelDisplay? Display { get; }

    RotationType DisplayRotation { get; }

    INetworkAdapter? NetworkAdapter { get; }

    ITemperatureSensor? TemperatureSensor { get; }

    IBarometricPressureSensor? BarometricPressureSensor { get; }

    IHumiditySensor? HumiditySensor { get; }
}