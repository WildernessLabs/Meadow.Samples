using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Rotary;
using Meadow.Peripherals;
using Meadow.Peripherals.Sensors.Rotary;
using System;
using System.Threading.Tasks;

namespace EdgeASketch
{
    // public class MeadowApp : App<F7FeatherV1> <- If you have a Meadow F7v1.*
    public class MeadowApp : App<F7FeatherV2>
    {
        int x, y;
        MicroGraphics graphics;
        RotaryEncoderWithButton rotaryX;
        RotaryEncoderWithButton rotaryY;

        public override Task Initialize()
        {
            var onboardLed = new RgbPwmLed(
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue);
            onboardLed.SetColor(Color.Red);

            x = y = 120;

            var st7789 = new St7789(
                spiBus: Device.CreateSpiBus(),
                chipSelectPin: null,
                dcPin: Device.Pins.D01,
                resetPin: Device.Pins.D00,
                width: 240, height: 240);

            graphics = new MicroGraphics(st7789);
            graphics.Clear(true);
            graphics.DrawRectangle(0, 0, 240, 240, Color.White, true);
            graphics.DrawPixel(x, y, Color.Red);
            graphics.Show();

            rotaryX = new RotaryEncoderWithButton(Device.Pins.A00, Device.Pins.A01, Device.Pins.A02);
            rotaryX.Rotated += RotaryXRotated;

            rotaryY = new RotaryEncoderWithButton(Device.Pins.D01, Device.Pins.D03, Device.Pins.D04);
            rotaryY.Rotated += RotaryYRotated;
            rotaryY.Clicked += RotaryYClicked;

            onboardLed.SetColor(Color.Green);

            return base.Initialize();
        }

        void RotaryXRotated(object sender, RotaryChangeResult e)
        {
            if (e.New == RotationDirection.Clockwise)
                x++;
            else
                x--;

            if (x > 239) x = 239;
            else if (x < 0) x = 0;

            graphics.DrawPixel(x, y + 1, Color.Red);
            graphics.DrawPixel(x, y, Color.Red);
            graphics.DrawPixel(x, y - 1, Color.Red);
            graphics.Show();
        }

        void RotaryYRotated(object sender, RotaryChangeResult e)
        {
            if (e.New == RotationDirection.Clockwise)
                y++;
            else
                y--;

            if (y > 239) y = 239;
            else if (y < 0) y = 0;

            graphics.DrawPixel(x + 1, y, Color.Red);
            graphics.DrawPixel(x, y, Color.Red);
            graphics.DrawPixel(x - 1, y, Color.Red);
            graphics.Show();
        }

        void RotaryYClicked(object sender, EventArgs e)
        {
            x = y = 120;

            graphics.DrawRectangle(0, 0, 240, 240, Color.White, true);
            graphics.DrawPixel(x, y, Color.Red);
            graphics.Show();
        }
    }
}