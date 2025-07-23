using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Leds;
using Meadow.Peripherals.Sensors.Motion;

namespace MagicEightMeadow.Hardware;

internal interface IMagicEightMeadowHardware
{
    IPixelDisplay Display { get; }

    IRgbPwmLed RgbPwmLed { get; }

    IAccelerometer MotionSensor { get; set; }

    void Initialize();
}