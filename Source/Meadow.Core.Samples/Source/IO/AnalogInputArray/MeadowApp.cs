using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AnalogInputArray;

public class MeadowApp : App<F7FeatherV2>
{
    private IAnalogInputArray array;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initializing hardware...");

        array = Device.CreateAnalogInputArray(Device.Pins.A00, Device.Pins.A01, Device.Pins.A02);

        Resolver.Log.Info("Hardware initialized.");

        return base.Initialize();
    }

    public override async Task Run()
    {
        var readsPerIteration = 1000;
        var a0 = new double[readsPerIteration];
        var a1 = new double[readsPerIteration];
        var a2 = new double[readsPerIteration];

        int start, et;

        while (true)
        {
            start = Environment.TickCount;

            // read 1k samples as fast as we can
            for (var i = 0; i < readsPerIteration; i++)
            {
                array.Refresh();
                a0[i] = array.CurrentValues[0];
                a1[i] = array.CurrentValues[1];
                a2[i] = array.CurrentValues[2];
            }

            et = Environment.TickCount - start;
            var sps = readsPerIteration * 3000 / (float)et; // 3 channels, 1k ms/sec

            // calculate means
            var a0Mean = a0.Average();
            var a1Mean = a1.Average();
            var a2Mean = a2.Average();

            Resolver.Log.Info($"Reading {sps:0} samples per second");
            Resolver.Log.Info($"  Means: A0={a0Mean:0.0} A1={a1Mean:0.0} A2={a2Mean:0.0}");

            // wait
            await Task.Delay(5000);
        }
    }
}