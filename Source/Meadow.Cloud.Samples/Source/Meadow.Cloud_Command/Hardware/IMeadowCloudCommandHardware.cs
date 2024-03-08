using Meadow.Foundation.Grove.Relays;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Leds;

namespace Meadow.Cloud_Command.Hardware
{
    internal interface IMeadowCloudCommandHardware
    {
        public IPixelDisplay Display { get; }

        public IRgbPwmLed RgbPwmLed { get; }

        public FourChannelSpdtRelay FourChannelRelay { get; }

        public void Initialize();
    }
}