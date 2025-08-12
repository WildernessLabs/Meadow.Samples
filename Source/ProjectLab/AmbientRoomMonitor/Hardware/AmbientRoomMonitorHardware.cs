using Meadow.Devices;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Leds;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Atmospheric;
using Meadow.Peripherals.Sensors.Light;

namespace AmbientRoomMonitor.Hardware;

internal class AmbientRoomMonitorHardware : IAmbientRoomMonitorHardware
{
    protected IProjectLabHardware ProjLab { get; private set; }

    public IPixelDisplay Display { get; set; }

    public IRgbPwmLed RgbPwmLed { get; set; }

    public ILightSensor LightSensor { get; set; }

    public ITemperatureSensor TemperatureSensor { get; set; }

    public IBarometricPressureSensor BarometricPressureSensor { get; set; }

    public IHumiditySensor HumiditySensor { get; set; }

    internal AmbientRoomMonitorHardware(IProjectLabHardware projectLab)
    {
        ProjLab = projectLab;
    }

    public void Initialize()
    {
        Display = ProjLab.Display;

        RgbPwmLed = ProjLab.RgbLed;

        LightSensor = ProjLab.LightSensor;

        TemperatureSensor = ProjLab.TemperatureSensor;
        BarometricPressureSensor = ProjLab.BarometricPressureSensor;
        HumiditySensor = ProjLab.HumiditySensor;
    }
}