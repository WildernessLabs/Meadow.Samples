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
    private bool _bootAfterCrash = false;

    public override Task Initialize()
    {
        var reliabilityService = Resolver.Services.Get<IReliabilityService>();

        reliabilityService.MeadowSystemError += ReliabilityService_MeadowSystemError;

        _bootAfterCrash = true;

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

    private void ReliabilityService_MeadowSystemError(object sender, MeadowSystemErrorInfo e)
    {
        if (e is Esp32SystemErrorInfo espError)
        {
            Resolver.Log.Warn($"The ESP32 has had an error ({espError.StatusCode}).");

            switch (espError.StatusCode)
            {
                case StatusCodes.EspOutOfMemory:
                    Resolver.Log.Error($"This is a fatal error. Resetting the device...");
                    Resolver.Device.PlatformOS.Reset();
                    break;
                default:
                    // any ESP reset error code is also fatal
                    if (espError.StatusCode >= StatusCodes.EspReset && espError.StatusCode <= StatusCodes.EspResetSDIO)
                    {
                        Resolver.Log.Error($"This is a fatal error. Resetting the device...");
                        Resolver.Device.PlatformOS.Reset();
                    }
                    break;
            }
        }
        else
        {
            Resolver.Log.Info($"We've had a system error: {e}");
        }
    }

    public override async Task Run()
    {
        if (!_bootAfterCrash)
        {
            await Task.Delay(TimeSpan.FromSeconds(_random.Next(3, 20)));
            Resolver.Log.Info("FORCING A CRASH!");
            throw new Exception($"OMG! My App Died with a random code {_random.Next(1, 101)}");
        }
        else
        {
            Resolver.Log.Info("Waiting for an ESP reset");

            //                CreateAnOOMError();
        }
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