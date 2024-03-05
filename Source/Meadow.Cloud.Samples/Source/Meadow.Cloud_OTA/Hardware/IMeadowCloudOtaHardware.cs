using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Leds;

namespace Meadow.Cloud_OTA.Hardware
{
    internal interface IMeadowCloudOtaHardware
    {
        public IPixelDisplay Display { get; }

        public IRgbPwmLed RgbPwmLed { get; }

        public void Initialize();
    }
}
