using Meadow.Devices;
using Meadow.Foundation.Sensors.Accelerometers;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Leds;

namespace MagicEightMeadow.Hardware
{
    internal class MagicEightMeadowHardware : IMagicEightMeadowHardware
    {
        protected IProjectLabHardware projectLab { get; private set; }

        public IPixelDisplay Display { get; set; }

        public IRgbPwmLed RgbPwmLed { get; set; }

        public Bmi270 MotionSensor { get; set; }

        public void Initialize()
        {
            projectLab = ProjectLab.Create();

            Display = projectLab.Display;

            RgbPwmLed = projectLab.RgbLed;

            MotionSensor = (projectLab as ProjectLabHardwareBase).MotionSensor;
        }
    }
}