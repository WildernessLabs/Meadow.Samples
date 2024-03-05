using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Watchdog
{
    public class MeadowApp : App<F7FeatherV2>
    {
        private IDigitalOutputPort blue;
        private IDigitalOutputPort red;
        private IDigitalOutputPort green;

        public override Task Initialize()
        {
            Resolver.Log.Info("===== Meadow Power Management Sample =====");
            Resolver.Log.Info($"{Device.Information.Platform} OS v.{Device.Information.OSVersion}");

            blue = Device.CreateDigitalOutputPort(Device.Pins.OnboardLedBlue, false);
            red = Device.CreateDigitalOutputPort(Device.Pins.OnboardLedRed, false);
            green = Device.CreateDigitalOutputPort(Device.Pins.OnboardLedGreen, false);

            Device.PlatformOS.BeforeSleep += () =>
            {
                for (var i = 0; i < 3; i++)
                {
                    green.State = red.State = blue.State = true;
                    Thread.Sleep(1000);
                    green.State = red.State = blue.State = false;
                    Thread.Sleep(1000);
                }

                Resolver.Log.Info("Device is about to enter Sleep mode");
                // actual serial output is asynchronous, so we need to delay a little to see the output
                Thread.Sleep(500);
            };

            Device.PlatformOS.AfterWake += (e, o) =>
            {
                green.State = true;
                red.State = blue.State = false;
                Thread.Sleep(1000);
                red.State = true;
                green.State = blue.State = false;
                Thread.Sleep(1000);
                blue.State = true;
                red.State = green.State = false;
                Thread.Sleep(1000);
                green.State = red.State = blue.State = false;

                Resolver.Log.Info("Device has returned from Sleep mode");
            };

            Device.PlatformOS.BeforeReset += () =>
            {
                Resolver.Log.Info("Device is about to Reset");

                green.State = true;
                red.State = true;
                blue.State = true;

                // actual serial output is asynchronous, so we need to delay a little to see the output
                Thread.Sleep(500);
            };

            return Task.CompletedTask;
        }

        public override async Task Run()
        {
            // blink blue pre-sleep
            var led = blue;

            // we'll run a loop for a while, outputting the time
            for (var i = 0; i < 13; i++)
            {
                Resolver.Log.Info($"Time is now: {DateTime.UtcNow:HH:mm:ss}");

                led.State = true;
                await Task.Delay(TimeSpan.FromMilliseconds(500));
                led.State = false;
                await Task.Delay(TimeSpan.FromMilliseconds(500));

                // then we'll sleep for 5 seconds
                if (i == 6)
                {
                    Device.PlatformOS.Sleep(TimeSpan.FromSeconds(5));

                    // swap to blink red for post-sleep
                    led = red;

                    // Don't use the console for a while after wake due to a bug that will crash the OS
                    Thread.Sleep(3000);
                }

                // when we wake we'll output a few more ticks
            }

            // finally we'll reset the device
            Device.PlatformOS.Reset();

            // we'll never reach here because of the reset above
        }
    }
}