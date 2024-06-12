using Meadow;
using Meadow.Devices;
using Meadow.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeadowF7_Logging;

public class FeatherApp : MeadowApp<F7FeatherV2> { }
public class CoreComputeApp : MeadowApp<F7CoreComputeV2> { }
public class MeadowApp<T> : App<T>
    where T : F7MicroBase
{
    public override async Task Initialize()
    {
        Resolver.Log.Info($"Initializing...");

        var cloudLogger = new CloudLogger();
        Resolver.Log.AddProvider(cloudLogger);
        Resolver.Services.Add(cloudLogger);

        var count = 1;
        var r = new Random();

        while (true)
        {
            // send a cloud log
            Resolver.Log.Info($"log loop {count++}");

            // send a cloud event
            var cl = Resolver.Services.Get<CloudLogger>();
            cl.LogEvent(0, "my first event", new Dictionary<string, object>()
            {
                { "temperature", r.Next(80, 110) },
                { "city", "log angeles" }
            });

            await Task.Delay(60 * 1000);
        }
    }
}