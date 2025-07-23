using Meadow.Devices;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Leds;
using Meadow.Peripherals.Sensors.Motion;

namespace MagicEightMeadow.Hardware;

internal class MagicEightMeadowHardware : IMagicEightMeadowHardware
{
    protected IProjectLabHardware ProjLab { get; }

    public IPixelDisplay Display { get; set; }

    public IRgbPwmLed RgbPwmLed { get; set; }

    public IAccelerometer MotionSensor { get; set; }

    public MagicEightMeadowHardware(IProjectLabHardware projectLab)
    {
        this.ProjLab = projectLab;
    }

    public void Initialize()
    {
        Display = ProjLab.Display;

        RgbPwmLed = ProjLab.RgbLed;

        MotionSensor = ProjLab.Accelerometer;
    }
}