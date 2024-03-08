using Meadow.Foundation.Sensors.Accelerometers;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Leds;

namespace MagicEightMeadow.Hardware
{
    internal interface IMagicEightMeadowHardware
    {
        public IPixelDisplay Display { get; }

        public IRgbPwmLed RgbPwmLed { get; }

        public Bmi270 MotionSensor { get; }

        public void Initialize();
    }
}