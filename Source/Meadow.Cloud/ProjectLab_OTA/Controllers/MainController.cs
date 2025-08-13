using Meadow;
using Meadow.Hardware;
using Meadow.Update;
using ProjectLab_OTA.Hardware;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectLab_OTA.Controllers;

internal class MainController
{
    private IMeadowCloudOtaHardware hardware;
    private IWiFiNetworkAdapter network;
    private DisplayController displayController;

    public MainController(IMeadowCloudOtaHardware hardware, IWiFiNetworkAdapter network)
    {
        this.hardware = hardware;
        this.network = network;
    }

    public void Initialize()
    {
        hardware.Initialize();

        displayController = new DisplayController(hardware.Display);
    }

    public Task Run()
    {
        var updateService = Resolver.UpdateService;

        updateService.StateChanged += OnUpdateStateChanged;

        updateService.RetrieveProgress += OnUpdateProgress;

        updateService.UpdateAvailable += OnUpdateAvailable;

        updateService.UpdateRetrieved += OnUpdateRetrieved;

        var cloudService = Resolver.MeadowCloudService;

        cloudService.ConnectionStateChanged += OnCloudStateChanged;

        return Task.CompletedTask;
    }

    private void OnUpdateStateChanged(object sender, UpdateState e)
    {
        displayController.UpdateStatus($"{FormatStatusMessage(e)}");
    }

    private void OnUpdateProgress(IUpdateService updateService, UpdateInfo info, CancellationTokenSource token)
    {
        short percentage = (short)(((double)info.DownloadProgress / info.FileSize) * 100);

        displayController.UpdateDownloadProgress(percentage);
    }

    private async void OnUpdateAvailable(IUpdateService updateService, UpdateInfo info, CancellationTokenSource token)
    {
        _ = hardware.RgbPwmLed.StartBlink(Color.Magenta);
        displayController.UpdateStatus("Update available!");
    }

    private async void OnUpdateRetrieved(IUpdateService updateService, UpdateInfo info, CancellationTokenSource token)
    {
        _ = hardware.RgbPwmLed.StartBlink(Color.Cyan);
        displayController.UpdateStatus("Update retrieved!");
    }

    private void OnCloudStateChanged(object sender, CloudConnectionState e)
    {
        displayController.UpdateStatus($"{FormatStatusMessage(e)}");
    }

    private string FormatStatusMessage(UpdateState state)
    {
        string message = string.Empty;

        switch (state)
        {
            case UpdateState.Dead: message = "Failed"; break;
            case UpdateState.Disconnected: message = "Disconnected"; break;
            case UpdateState.Connected: message = "Connected!"; break;
            case UpdateState.DownloadingFile: message = "Downloading File..."; break;
            case UpdateState.UpdateInProgress: message = "Update In Progress..."; break;
        }

        return message;
    }

    private string FormatStatusMessage(CloudConnectionState state)
    {
        string message = string.Empty;

        switch (state)
        {
            case CloudConnectionState.Disconnected: message = "Disconnected"; break;
            case CloudConnectionState.Authenticating: message = "Authenticating..."; break;
            case CloudConnectionState.Connecting: message = "Connecting..."; break;
            case CloudConnectionState.Connected: message = "Connected!"; break;
        }

        return message;
    }
}