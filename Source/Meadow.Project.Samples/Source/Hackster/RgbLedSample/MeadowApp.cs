using System;
using System.Threading;
using System.Threading.Tasks;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Peripherals.Leds;

namespace RgbLedSample
{
    // public class MeadowApp : App<F7FeatherV1> <- If you have a Meadow F7v1.*
    public class MeadowApp : App<F7FeatherV2>
    {
        RgbLed rgbLed;
        RgbPwmLed rgbPwmLed;

        public override Task Initialize() 
        {
            var onboardLed = new RgbPwmLed(
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue);
            onboardLed.SetColor(Color.Red);

            rgbLed = new RgbLed(
                Device.Pins.D02,
                Device.Pins.D03,
                Device.Pins.D04
            );
            //rgbPwmLed = new RgbPwmLed(
            //    Device.Pins.D02,
            //    Device.Pins.D03,
            //    Device.Pins.D04
            //);

            onboardLed.SetColor(Color.Green);

            return base.Initialize();
        }

        async Task TestRgbLed()
        {
            Console.WriteLine("TestRgbLed...");

            while (true)
            {
                Console.WriteLine("Going through each color on each RGB LED...");
                for (int i = 0; i < (int)RgbLedColors.count; i++)
                {
                    rgbLed.SetColor((RgbLedColors)i);
                    await Task.Delay(500);
                }
            }
        }

        async Task TestRgbPwmLed()
        {
            Console.WriteLine("TestRgbPwmLed...");

            while (true)
            {
                for (int i = 0; i < 360; i++)
                {
                    var hue = ((double)i / 360F);
                    rgbPwmLed.SetColor(Color.FromHsba(((double)i / 360F), 1, 1));
                    Console.WriteLine($"Hue {hue}");
                    await Task.Delay(25);
                }
            }
        }

        public override async Task Run()
        {
            await TestRgbLed();
            await TestRgbPwmLed();
        }
    }
}