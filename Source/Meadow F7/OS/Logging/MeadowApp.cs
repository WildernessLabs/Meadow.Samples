using Meadow;
using Meadow.Devices;
using System;
using System.Threading.Tasks;

namespace Logging;

public class MeadowApp : App<F7FeatherV2>
{
    public override Task Initialize()
    {
        Resolver.Log.Info($"Initializing...");

        var fileLogger = new FileLogger();

        // output the log contents just for display.  Do it before adding the logger so we don't recurse
        var lineNumber = 1;
        var contents = fileLogger.GetLogContents();
        if (contents.Length > 0)
        {
            Resolver.Log.Info($"Log contents{Environment.NewLine}------------");

            foreach (var line in contents)
            {
                Resolver.Log.Info($"{lineNumber++:000}> {line}");
            }
            Resolver.Log.Info($"------------");
        }
        else
        {
            Resolver.Log.Info($"Log is empty");
        }

        // an our own logger to the system logger
        Resolver.Log.AddProvider(fileLogger);

        return base.Initialize();
    }

    public override Task Run()
    {
        Resolver.Log.Info("This will not end up in the file");

        // prefix a random number just so we can see differences
        var r = new Random();
        Resolver.Log.Warn($"But this will [{r.Next(0, 1000)}]");

        return Task.CompletedTask;
    }
}