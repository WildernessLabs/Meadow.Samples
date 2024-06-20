using AmbientMonitor.Core.Contracts;
using Meadow;
using Meadow.Foundation.Sensors;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Foundation.Sensors.Hid;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Atmospheric;
using Meadow.Peripherals.Sensors.Buttons;
using Meadow.Units;

namespace AmbientMonitor.DesktopApp.Hardware;

public class AmbientMonitorHardware : IAmbientMonitorHardware
{
    private readonly Desktop device;

    public IButton? LeftButton { get; }

    public IButton? RightButton { get; }

    public IPixelDisplay? Display => device.Display;

    public RotationType DisplayRotation => RotationType.Default;

    public INetworkAdapter? NetworkAdapter { get; }

    public ITemperatureSensor? TemperatureSensor { get; }

    public IBarometricPressureSensor? BarometricPressureSensor { get; }

    public IHumiditySensor? HumiditySensor { get; }

    public AmbientMonitorHardware(Desktop device)
    {
        this.device = device;

        var keyboard = new Keyboard();

        LeftButton = new PushButton(keyboard.Pins.Left.CreateDigitalInterruptPort(InterruptMode.EdgeFalling));

        RightButton = new PushButton(keyboard.Pins.Right.CreateDigitalInterruptPort(InterruptMode.EdgeFalling));

        TemperatureSensor = new SimulatedTemperatureSensor(
            new Temperature(20, Temperature.UnitType.Celsius),
            new Temperature(18, Temperature.UnitType.Celsius),
            new Temperature(25, Temperature.UnitType.Celsius));

        BarometricPressureSensor = new SimulatedBarometricPressureSensor();

        HumiditySensor = new SimulatedHumiditySensor();

        if (MeadowApp.Device.NetworkAdapters.Count > 0)
        {
            NetworkAdapter = MeadowApp.Device.NetworkAdapters[0];
        }
    }
}