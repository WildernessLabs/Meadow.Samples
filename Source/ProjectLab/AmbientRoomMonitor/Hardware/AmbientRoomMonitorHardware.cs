using Meadow.Devices;
using Meadow.Foundation.Sensors.Atmospheric;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Leds;
using Meadow.Peripherals.Sensors.Light;

namespace AmbientRoomMonitor.Hardware;

internal class AmbientRoomMonitorHardware : IAmbientRoomMonitorHardware
{
    protected IProjectLabHardware ProjLab { get; private set; }

    public IPixelDisplay Display { get; set; }

    public IRgbPwmLed RgbPwmLed { get; set; }

    public ILightSensor LightSensor { get; set; }

    public Bme68x EnvironmentalSensor { get; set; }

    internal AmbientRoomMonitorHardware(IProjectLabHardware projectLab)
    {
        ProjLab = projectLab;
    }

    public void Initialize()
    {
        Display = ProjLab.Display;

        RgbPwmLed = ProjLab.RgbLed;

        LightSensor = ProjLab.LightSensor;

        EnvironmentalSensor = (ProjLab as ProjectLabHardwareBase).AtmosphericSensor;
    }
}