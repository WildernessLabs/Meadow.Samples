using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Hid;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using System.Threading.Tasks;

namespace TouchKeypad
{
    // public class MeadowApp : App<F7FeatherV1> <- If you have a Meadow F7v1.*
    public class MeadowApp : App<F7FeatherV2>
    {
        Mpr121 sensor;
        MicroGraphics graphics;

        public override Task Initialize()
        {
            var onboardLed = new RgbPwmLed(
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue);
            onboardLed.SetColor(Color.Red);

            var display = new St7789(
                spiBus: Device.CreateSpiBus(),
                chipSelectPin: Device.Pins.D02,
                dcPin: Device.Pins.D01,
                resetPin: Device.Pins.D00,
                width: 240, height: 240);

            graphics = new MicroGraphics(display)
            {
                Stroke = 2,
                Rotation = RotationType._180Degrees,
                CurrentFont = new Font12x16(),
            };

            sensor = new Mpr121(Device.CreateI2cBus(I2cBusSpeed.Standard), 90, 100);
            sensor.ChannelStatusesChanged += SensorChannelStatusesChanged;

            onboardLed.SetColor(Color.Green);

            return base.Initialize();
        }

        void DrawGrid()
        {
            graphics.Clear();
            for (int columns = 0; columns < 3; columns++)
            {
                for (int rows = 3; rows >= 0; rows--)
                {
                    graphics.DrawRectangle(columns * 57 + 38, rows * 57 + 10, 51, 51, Color.Cyan);
                }
            }
            graphics.Show();
        }

        void SensorChannelStatusesChanged(object sender, ChannelStatusChangedEventArgs e)
        {
            graphics.Clear();
            graphics.Stroke = 1;

            for (int i = 0; i < e.ChannelStatus.Count; i++)
            {
                int numpadIndex = 0;
                for (int columns = 0; columns < 3; columns++)
                {
                    for (int rows = 3; rows >= 0; rows--)
                    {
                        if (numpadIndex == i)
                        {
                            if (e.ChannelStatus[(Mpr121.Channels)i])
                                graphics.DrawRectangle(columns * 57 + 38, rows * 57 + 10, 51, 51, Color.Cyan, true);
                            else
                                graphics.DrawRectangle(columns * 57 + 38, rows * 57 + 10, 51, 51, Color.Cyan);
                        }
                        numpadIndex++;
                    }
                }
            }

            graphics.Show();
        }

        public override Task Run()
        {
            DrawGrid();

            return base.Run();
        }
    }
}