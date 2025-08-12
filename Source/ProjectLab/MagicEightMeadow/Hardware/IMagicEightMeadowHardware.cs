using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Leds;
using Meadow.Peripherals.Sensors.Motion;

namespace MagicEightMeadow.Hardware;

internal interface IMagicEightMeadowHardware
{
    public IPixelDisplay Display { get; }

    public IRgbPwmLed RgbPwmLed { get; }

    public IAccelerometer MotionSensor { get; }

    public void Initialize();
}