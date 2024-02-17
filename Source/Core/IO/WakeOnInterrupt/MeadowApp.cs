using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WakeOnInterrupt
{
    /// <summary>
    /// This sample illustrates putting the device into low-power (sleep) state until an interrupt occurs
    /// </summary>
    public class MeadowApp : App<F7FeatherV2>
    {
        private IDigitalOutputPort _red;
        private IDigitalOutputPort _green;
        private IDigitalOutputPort _blue;

        public TimeSpan Timespan { get; private set; }

        public override Task Initialize()
        {
            Resolver.Log.Info("Initializing hardware...");

            _red = Device.Pins.OnboardLedRed.CreateDigitalOutputPort(false);
            _green = Device.Pins.OnboardLedGreen.CreateDigitalOutputPort(false);
            _blue = Device.Pins.OnboardLedBlue.CreateDigitalOutputPort(false);

            Device.PlatformOS.BeforeSleep += () =>
            {
                _red.State = true;
                Resolver.Log.Info("Sleeping...");
            };

            Device.PlatformOS.AfterWake += (s, e) =>
            {
                Resolver.Log.Info($"Wake reason: {e}");

                _blue.State = true;
                Thread.Sleep(1000);
                _red.State = false;
                Resolver.Log.Info("Resuming...");

            };

            return Task.CompletedTask;
        }

        public override async Task Run()
        {
            while (true)
            {
                Resolver.Log.Info("Waiting...");
                for (var i = 0; i < 5; i++)
                {
                    _green.State = true;
                    await Task.Delay(500);
                    _green.State = false;
                    await Task.Delay(500);
                }

                Resolver.Log.Info("g'night!");
                Device.PlatformOS.Sleep(Device.Pins.D05, InterruptMode.EdgeRising, ResistorMode.ExternalPullDown);

                Thread.Sleep(1000);
                _blue.State = false;
                Resolver.Log.Info("Continuing...");
            }
        }
    }
}