using AmbientMonitor.Core.Contracts;
using Meadow;
using Meadow.Foundation.Sensors;
using Meadow.Foundation.Sensors.Hid;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors;
using Meadow.Units;

namespace AmbientMonitor.DesktopApp.Hardware;

internal class AmbientMonitorHardware : IAmbientMonitorHardware
{
    private readonly Desktop device;
    private readonly Keyboard keyboard;

    public RotationType DisplayRotation => RotationType.Default;

    public IPixelDisplay? Display => device.Display;

    public ITemperatureSensor? TemperatureSensor { get; }

    public AmbientMonitorHardware(Desktop device)
    {
        this.device = device;

        keyboard = new Keyboard();

        TemperatureSensor = new SimulatedTemperatureSensor(
            new Temperature(70, Temperature.UnitType.Fahrenheit),
            keyboard.Pins.Up.CreateDigitalInterruptPort(InterruptMode.EdgeRising),
            keyboard.Pins.Down.CreateDigitalInterruptPort(InterruptMode.EdgeRising));
    }
}