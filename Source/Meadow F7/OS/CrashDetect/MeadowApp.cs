using Meadow;
using Meadow.Devices;
using Meadow.Devices.Esp32.MessagePayloads;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CrashDetectTest;

public class FeatherApp : MeadowApp<F7FeatherV2> { }
public class CoreComputeApp : MeadowApp<F7CoreComputeV2> { }

public class MeadowApp<T> : App<T>
    where T : F7MicroBase
{
    private Random _random = new();

    public override Task Initialize()
    {
        var reliabilityService = Resolver.Services.Get<IReliabilityService>();

        //        reliabilityService.MeadowSystemError += OnMeadowSystemError;

        if (reliabilityService.LastBootWasFromCrash)
        {
            Resolver.Log.Info("Booting after a crash!");

            Resolver.Log.Info("Crash report:");
            foreach (var r in reliabilityService.GetCrashData())
            {
                Resolver.Log.Info(r);
            }

            Resolver.Log.Info("Clearing crash data...");
            reliabilityService.ClearCrashData();
        }

        return base.Initialize();
    }

    private void OnMeadowSystemError(MeadowSystemErrorInfo error, bool recommendReset, out bool forceReset)
    {
        if (error is Esp32SystemErrorInfo espError)
        {
            Resolver.Log.Warn($"The ESP32 has had an error ({espError.StatusCode}).");
        }
        else
        {
            Resolver.Log.Info($"We've had a system error: {error}");
        }

        if (recommendReset)
        {
            Resolver.Log.Warn($"Meadow is recommending a device reset");
        }

        forceReset = recommendReset;

        // override the reset recommendation
        //forceReset = false;
    }

    public override async Task Run()
    {
        await Task.Delay(TimeSpan.FromSeconds(_random.Next(3, 20)));
        Resolver.Log.Info("FORCING A CRASH!");
        //        throw new Exception($"OMG! My App Died with a random code {_random.Next(1, 101)}");
    }

    private void CreateAnOOMError()
    {
        Resolver.Log.Info("Please wait.  Buffering....");
        var list = new List<byte>();

        var size = 1;

        while (true)
        {
            var buffer = new byte[size];
            _random.NextBytes(buffer);
            list.AddRange(buffer);
            size *= 2;
            Thread.Sleep(100);
        }
    }
}