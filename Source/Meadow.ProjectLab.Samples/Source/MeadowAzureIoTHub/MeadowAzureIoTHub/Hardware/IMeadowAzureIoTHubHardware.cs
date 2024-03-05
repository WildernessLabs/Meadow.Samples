using Meadow.Foundation.Sensors.Atmospheric;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Leds;

namespace MeadowAzureIoTHub.Hardware
{
    internal interface IMeadowAzureIoTHubHardware
    {
        public IPixelDisplay Display { get; }

        public Bme68x EnvironmentalSensor { get; }

        public IRgbPwmLed RgbPwmLed { get; }

        public void Initialize();
    }
}