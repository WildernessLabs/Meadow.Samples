using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LedSample
{
    // public class MeadowApp : App<F7FeatherV1> <- If you have a Meadow F7v1.*
    public class MeadowApp : App<F7FeatherV2>
    {
        Led led;
        PwmLed pwmLed;

        public override Task Initialize()
        {
            led = new Led(Device.CreateDigitalOutputPort(Device.Pins.D01));
            //pwmLed = new PwmLed(Device.CreatePwmPort(Device.Pins.D02),
            //   TypicalForwardVoltage.Red);

            return base.Initialize();
        }

        async Task TestLed()
        {
            while (true)
            {
                Console.WriteLine("Turning on and off each led for 1 second");
                for (int i = 0; i < 2; i++)
                {
                    led.IsOn = true;
                    Thread.Sleep(1000);
                    led.IsOn = false;
                    Thread.Sleep(1000);
                }

                Console.WriteLine("Blinking the LED for a bit.");
                await led.StartBlink();
                Thread.Sleep(3000);
                await led.StopAnimation();
            }
        }

        async Task TestPwmLed()
        {
            Console.WriteLine("TestPwmLed...");

            while (true)
            {
                Console.WriteLine("Turning on and off each led for one second");
                pwmLed.IsOn = true;
                Thread.Sleep(500);
                pwmLed.IsOn = false;

                Console.WriteLine("Blinking the LED for three seconds...");
                await pwmLed.StartBlink();
                Thread.Sleep(3000);
                await pwmLed.StopAnimation();

                Console.WriteLine("Pulsing the LED for three seconds...");
                await pwmLed.StartPulse();
                Thread.Sleep(3000);
                await pwmLed.StopAnimation();

                Console.WriteLine("Increasing and decreasing brightness...");
                for (int j = 0; j <= 3; j++)
                {
                    for (int i = 0; i <= 10; i++)
                    {
                        pwmLed.SetBrightness(i * 0.10f);
                        Thread.Sleep(100);
                    }

                    for (int i = 10; i >= 0; i--)
                    {
                        pwmLed.SetBrightness(i * 0.10f);
                        Thread.Sleep(100);
                    }
                }
            }
        }

        public override async Task Run()
        {
            await TestLed();
            //await TestPwmLed();
        }
    }
}