using Meadow;
using Meadow.Devices;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Watchdog
{
    public class MeadowApp : App<F7FeatherV2>
    {
        public override Task Initialize()
        {
            Resolver.Log.Info("Initialize hardware...");

            // enable the watchdog for 10s
            Device.WatchdogEnable(TimeSpan.FromSeconds(10));
            StartPettingWatchdog(9000);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Starts up a thread that resets the watchdog at the specified interval.
        /// </summary>
        /// <param name="pettingInterval"></param>
        void StartPettingWatchdog(int pettingInterval)
        {
            // just for good measure, let's reset the watchdog to begin with
            Device.WatchdogReset();
            // start a thread that pets it
            Thread t = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(pettingInterval);
                    Resolver.Log.Info("Petting watchdog.");
                    Device.WatchdogReset();
                }
            });
            t.Start();
        }
    }
}