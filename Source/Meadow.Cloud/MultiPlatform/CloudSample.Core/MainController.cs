using Meadow;
using Meadow.Logging;
using System;
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

    private void OnUpdateRetrieved(Meadow.Update.IUpdateService updateService, Meadow.Update.UpdateInfo info, CancellationTokenSource token)
    {
        Resolver.Log.Info($"Update {info.ID} retrieved");
    }

    private void OnRetrieveProgress(Meadow.Update.IUpdateService updateService, Meadow.Update.UpdateInfo info, CancellationTokenSource token)
    {
        Resolver.Log.Info($"{info.ID} retrieved {info.DownloadProgress}");
    }

    private void OnUpdateAvailable(Meadow.Update.IUpdateService updateService, Meadow.Update.UpdateInfo info, CancellationTokenSource token)
    {
        Resolver.Log.Info($"An update is available: {info.ID}");
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
        //        telemetryController.LogTelemetry();

        controlTimer.Change(10000, -1);
    }
}
