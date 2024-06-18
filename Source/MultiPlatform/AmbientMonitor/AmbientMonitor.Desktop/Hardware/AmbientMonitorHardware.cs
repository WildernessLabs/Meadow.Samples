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

    public INetworkAdapter? NetworkAdapter { get; }

    public ITemperatureSensor? TemperatureSensor { get; }

    //public IBarometricPressureSensor? BarometricPressureSensor { get; }

    //public IHumiditySensor? HumiditySensor { get; }

    public AmbientMonitorHardware(Desktop device)
    {
        this.device = device;

        keyboard = new Keyboard();

        TemperatureSensor = new SimulatedTemperatureSensor(
            new Temperature(20, Temperature.UnitType.Celsius),
            new Temperature(18, Temperature.UnitType.Celsius),
            new Temperature(25, Temperature.UnitType.Celsius));

        if (MeadowApp.Device.NetworkAdapters.Count > 0)
        {
            NetworkAdapter = MeadowApp.Device.NetworkAdapters[0];
        }
    }
}