using AmbientMonitor.Core.Contracts;
using Meadow.Devices;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Atmospheric;
using Meadow.Peripherals.Sensors.Buttons;

namespace AmbientMonitor.F7;

public class AmbientMonitorProjectLabHardware : IAmbientMonitorHardware
{
    private readonly IProjectLabHardware projLab;

    public IButton? LeftButton => projLab.LeftButton;

    public IButton? RightButton => projLab.RightButton;

    public IPixelDisplay? Display => projLab.Display;

    public RotationType DisplayRotation => RotationType._270Degrees;

    public INetworkAdapter? NetworkAdapter => projLab.ComputeModule.NetworkAdapters.Primary<IWiFiNetworkAdapter>();

    public ITemperatureSensor? TemperatureSensor => projLab.TemperatureSensor;

    public IBarometricPressureSensor? BarometricPressureSensor => projLab.BarometricPressureSensor;

    public IHumiditySensor? HumiditySensor => projLab.HumiditySensor;

    public AmbientMonitorProjectLabHardware(IProjectLabHardware projLab)
    {
        this.projLab = projLab;
    }
}