using Meadow;
using Meadow.Devices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrashDetectTest;

public class FeatherApp : App<F7FeatherV2> { }
public class CoreComputeApp : App<F7CoreComputeV2> { }

public class MeadowApp<T> : App<T>
    where T : F7MicroBase
{
    public override void OnBootFromCrash(IEnumerable<string> crashReports)
    {
        Resolver.Log.Info("Booting after a crash!");

        Resolver.Log.Info("Crash report:");
        foreach (var r in crashReports)
        {
            Resolver.Log.Info(r);
        }

        Resolver.Log.Info("Clearing crash data...");
        Resolver.Services.Get<CrashReporter>()?.ClearCrashData();
    }

    public override async Task Run()
    {
        var r = new Random();

        await Task.Delay(TimeSpan.FromSeconds(r.Next(3, 20)));

        Resolver.Log.Info("FORCING A CRASH!");
        throw new Exception($"OMG! My App Died with a random code {r.Next(1, 101)}");
    }
}