using Meadow.Cloud_OTA.Hardware;
using Meadow.Hardware;
using Meadow.Update;
using System.Threading.Tasks;

namespace Meadow.Cloud_OTA.Controllers
{
    internal class MainController
    {
        IMeadowCloudOtaHardware hardware;
        IWiFiNetworkAdapter network;
        DisplayController displayController;

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
            var svc = Resolver.Services.Get<IUpdateService>() as UpdateService;
            svc.ClearUpdates(); // uncomment to clear persisted info

            svc.OnStateChanged += SvcOnStateChanged;

            svc.OnUpdateProgress += SvcOnUpdateProgress;

            svc.OnUpdateAvailable += SvcOnUpdateAvailable;

            svc.OnUpdateRetrieved += SvcOnUpdateRetrieved;

            return Task.CompletedTask;
        }

        private void SvcOnStateChanged(object sender, UpdateState e)
        {
            displayController.UpdateStatus($"{FormatStatusMessage(e)}");
        }

        private void SvcOnUpdateProgress(IUpdateService updateService, UpdateInfo info)
        {
            short percentage = (short)(((double)info.DownloadProgress / info.FileSize) * 100);

            displayController.UpdateDownloadProgress(percentage);
        }

        private async void SvcOnUpdateAvailable(IUpdateService updateService, UpdateInfo info)
        {
            _ = hardware.RgbPwmLed.StartBlink(Color.Magenta);
            displayController.UpdateStatus("Update available!");

            await Task.Delay(5000);
            updateService.RetrieveUpdate(info);
        }

        private async void SvcOnUpdateRetrieved(IUpdateService updateService, UpdateInfo info)
        {
            _ = hardware.RgbPwmLed.StartBlink(Color.Cyan);
            displayController.UpdateStatus("Update retrieved!");

            await Task.Delay(5000);
            updateService.ApplyUpdate(info);
        }

        private string FormatStatusMessage(UpdateState state)
        {
            string message = string.Empty;

            switch (state)
            {
                case UpdateState.Dead: message = "Failed"; break;
                case UpdateState.Disconnected: message = "Disconnected"; break;
                case UpdateState.Authenticating: message = "Authenticating..."; break;
                case UpdateState.Connecting: message = "Connecting..."; break;
                case UpdateState.Connected: message = "Connected!"; break;
                case UpdateState.Idle: message = "Ready!"; break;
                case UpdateState.UpdateAvailable: message = "Update Available!"; break;
                case UpdateState.DownloadingFile: message = "Downloading File..."; break;
                case UpdateState.UpdateInProgress: message = "Update In Progress..."; break;
            }

            return message;
        }
    }
}