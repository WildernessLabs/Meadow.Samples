using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Threading_Basics
{
    public class MeadowApp : App<F7FeatherV2>
    {
        IDigitalOutputPort out1;
        IDigitalOutputPort out2;

        public override Task Initialize()
        {
            out1 = Device.CreateDigitalOutputPort(Device.Pins.D00);
            out2 = Device.CreateDigitalOutputPort(Device.Pins.D01);

            out1.State = true;

            return Task.CompletedTask;
        }

        public override Task Run()
        {
            Task.Run(() =>
            {
                Resolver.Log.Info($"Running");

                var i = 0;

                while (true)
                {
                    out2.State = true;
                    Thread.Sleep(250);
                    out2.State = false;
                    Thread.Sleep(250);

                    if (i++ % 20 == 0)
                    {
                        long memUsed = GC.GetTotalMemory(false);
                        Resolver.Log.Info($"GC: {DateTime.Now:yyyy/MM/dd HH:mm:ss}  {memUsed:N0}");
                    }
                }

            });

            return Task.CompletedTask;
        }
    }
}