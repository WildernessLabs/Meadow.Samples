using Meadow;
using Meadow.Cloud;
using Meadow.Logging;
using System;
using System.Collections.Generic;
using System.Threading;

namespace CloudSample;

public class MainController
{
    private CommandController commandController;
    private TelemetryController telemetryController;
    private Timer controlTimer;
    private bool firstConnect = true;
    private bool simulateFatalCrash = false;

    public MainController()
    {
        Resolver.MeadowCloudService.ConnectionStateChanged += OnConnectionStateChanged;
        Resolver.UpdateService.UpdateAvailable += OnUpdateAvailable;
        Resolver.UpdateService.RetrieveProgress += OnRetrieveProgress;
        Resolver.UpdateService.UpdateRetrieved += OnUpdateRetrieved;

        commandController = new CommandController(Resolver.CommandService);
        telemetryController = new TelemetryController(Resolver.MeadowCloudService);
        controlTimer = new Timer(ControlTimerProc);
    }

    private void OnUpdateRetrieved(Meadow.Update.IUpdateService updateService, Meadow.Update.UpdateInfo info)
    {
        Resolver.Log.Info($"Update {info.ID} retrieved");
    }

    private void OnRetrieveProgress(Meadow.Update.IUpdateService updateService, Meadow.Update.UpdateInfo info)
    {
        Resolver.Log.Info($"{info.ID} retrieved {info.DownloadProgress}");
    }

    private void OnUpdateAvailable(Meadow.Update.IUpdateService updateService, Meadow.Update.UpdateInfo info)
    {
        Resolver.Log.Info($"An update is available: {info.ID}");

        Resolver.Log.Info($"retrieving...");
        updateService.RetrieveUpdate(info);
    }

    private void OnConnectionStateChanged(object sender, CloudConnectionState e)
    {
        Resolver.Log.Info($"Cloud connection is: {e}");

        if (firstConnect)
        {
            _ = Resolver.MeadowCloudService?.SendLog(LogLevel.Information, "CloudSample Started");
        }
    }

    public void Start()
    {
        controlTimer.Change(1000, -1);

        if (simulateFatalCrash)
        {
            new Thread(() =>
            {
                Thread.Sleep(2000);
                throw new Exception("APP FATALITY");
            }).Start();
        }
    }

    private void ControlTimerProc(object o)
    {
        telemetryController.LogTelemetry();

        controlTimer.Change(10000, -1);
    }
}

public class TelemetryController
{
    private const int DataEventId = 2000;

    private Random random = new();
    private IMeadowCloudService cloudService;

    public TelemetryController(IMeadowCloudService cloudService)
    {
        this.cloudService = cloudService;
    }

    public void LogTelemetry()
    {
        var data = new Dictionary<string, object>
        {
            { "Int Value", random.Next(43) },
            { "String Value", BitConverter.ToString(BitConverter.GetBytes(random.NextDouble())) }
        };

        cloudService.SendEvent(DataEventId, "CloudSample Data", data);
    }
}

public class CommandController
{
    public CommandController(ICommandService commandService)
    {
        commandService.Subscribe<SampleCommand>(OnSampleCommandRecevied);
    }

    private void OnSampleCommandRecevied(SampleCommand command)
    {
        Resolver.Log.Info($"Command received: Data = {command.Data}");
    }
}
