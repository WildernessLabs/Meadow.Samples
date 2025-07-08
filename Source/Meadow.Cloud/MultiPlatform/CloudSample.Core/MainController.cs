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

        Resolver.UpdateService.UpdateRetrieved += OnUpdateRetrieved;
        Resolver.UpdateService.RetrieveProgress += OnRetrieveProgress;
        Resolver.UpdateService.UpdateSuccess += OnUpdateSuccess;
        Resolver.UpdateService.UpdateFailure += OnUpdateFailure;

        commandController = new CommandController(Resolver.CommandService);
        telemetryController = new TelemetryController(Resolver.MeadowCloudService);
        controlTimer = new Timer(ControlTimerProc);
    }

    private void OnUpdateFailure(Meadow.Update.IUpdateService updateService, Meadow.Update.UpdateInfo info, CancellationTokenSource cancel)
    {
        Resolver.Log.Error($"Update Failure: {info.ID} - {info.Version} - {info.Detail}");
    }

    private void OnUpdateSuccess(Meadow.Update.IUpdateService updateService, Meadow.Update.UpdateInfo info, CancellationTokenSource cancel)
    {
        Resolver.Log.Info($"Update Success: {info.ID} - {info.Version}");
    }

    private void OnRetrieveProgress(Meadow.Update.IUpdateService updateService, Meadow.Update.UpdateInfo info, CancellationTokenSource cancel)
    {
        Resolver.Log.Info($"Update Retrieve Progress: {info.ID} - {info.Version} - {info.DownloadProgress}%");
    }

    private void OnUpdateRetrieved(Meadow.Update.IUpdateService updateService, Meadow.Update.UpdateInfo info, CancellationTokenSource cancel)
    {
        Resolver.Log.Info($"Update Retrieved: {info.ID} - {info.Version}");
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
