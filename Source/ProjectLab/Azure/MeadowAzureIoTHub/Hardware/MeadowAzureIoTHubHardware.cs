using Meadow.Devices;
using Meadow.Foundation.Sensors.Atmospheric;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Leds;

namespace MeadowAzureIoTHub.Hardware
{
    internal class MeadowAzureIoTHubHardware : IMeadowAzureIoTHubHardware
    {
        protected IProjectLabHardware ProjLab { get; private set; }

        public IPixelDisplay Display { get; set; }

        public Bme68x EnvironmentalSensor { get; set; }

        public IRgbPwmLed RgbPwmLed { get; set; }

        public void Initialize()
        {
            ProjLab = ProjectLab.Create();

            Display = ProjLab.Display;

            RgbPwmLed = ProjLab.RgbLed;

            EnvironmentalSensor = (ProjLab as ProjectLabHardwareBase).AtmosphericSensor;
        }
    }
}