using Meadow.Devices;
using Meadow.Foundation.Sensors.Atmospheric;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Leds;

namespace ProjectLab_AzureIoTHub.Hardware;

internal class MeadowAzureIoTHubHardware : IMeadowAzureIoTHubHardware
{
    protected IProjectLabHardware ProjLab { get; private set; }

    public IPixelDisplay Display { get; set; }

    public Bme68x EnvironmentalSensor { get; set; }

    public IRgbPwmLed RgbPwmLed { get; set; }

    public MeadowAzureIoTHubHardware(IProjectLabHardware projLab)
    {
        ProjLab = projLab;
    }

    public void Initialize()
    {
        Display = ProjLab.Display;

        RgbPwmLed = ProjLab.RgbLed;

        EnvironmentalSensor = (ProjLab as ProjectLabHardwareBase).AtmosphericSensor;
    }
}