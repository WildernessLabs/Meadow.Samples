using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Leds;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Atmospheric;

namespace Meadow.Cloud_Logging.Hardware
{
    internal interface IMeadowCloudLoggingHardware
    {
        public IPixelDisplay Display { get; }

        public ITemperatureSensor TemperatureSensor { get; set; }

        public IBarometricPressureSensor BarometricPressureSensor { get; set; }

        public IHumiditySensor HumiditySensor { get; set; }

        public IRgbPwmLed RgbPwmLed { get; }

        public void Initialize();
    }
}