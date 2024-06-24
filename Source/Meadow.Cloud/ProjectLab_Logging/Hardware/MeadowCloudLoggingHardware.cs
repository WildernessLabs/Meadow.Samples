using Meadow.Devices;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Leds;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Atmospheric;

namespace ProjectLab_Logging.Hardware;

internal class MeadowCloudLoggingHardware : IMeadowCloudLoggingHardware
{
    protected IProjectLabHardware ProjLab { get; }

    public IPixelDisplay Display { get; set; }

    public ITemperatureSensor TemperatureSensor { get; set; }

    public IBarometricPressureSensor BarometricPressureSensor { get; set; }

    public IHumiditySensor HumiditySensor { get; set; }

    public IRgbPwmLed RgbPwmLed { get; set; }

    public MeadowCloudLoggingHardware(IProjectLabHardware projLab)
    {
        ProjLab = projLab;
    }

    public void Initialize()
    {
        Display = ProjLab.Display;

        RgbPwmLed = ProjLab.RgbLed;

        TemperatureSensor = ProjLab.TemperatureSensor;

        BarometricPressureSensor = ProjLab.BarometricPressureSensor;

        HumiditySensor = ProjLab.HumiditySensor;
    }
}