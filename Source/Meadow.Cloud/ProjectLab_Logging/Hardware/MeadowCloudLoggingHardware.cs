using Meadow.Devices;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Leds;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Atmospheric;

namespace ProjectLab_Logging.Hardware;

internal class MeadowCloudLoggingHardware : IMeadowCloudLoggingHardware
{
    protected ProjectLabHardwareBase ProjectLab { get; private set; }

    public IPixelDisplay Display { get; set; }

    public ITemperatureSensor TemperatureSensor { get; set; }

    public IBarometricPressureSensor BarometricPressureSensor { get; set; }

    public IHumiditySensor HumiditySensor { get; set; }

    public IRgbPwmLed RgbPwmLed { get; set; }

    public void Initialize()
    {
        ProjectLab = Meadow.Devices.ProjectLab.Create() as ProjectLabHardwareBase;

        Display = ProjectLab.Display;

        RgbPwmLed = ProjectLab.RgbLed;

        TemperatureSensor = ProjectLab.TemperatureSensor;

        BarometricPressureSensor = ProjectLab.BarometricPressureSensor;

        HumiditySensor = ProjectLab.HumiditySensor;
    }
}