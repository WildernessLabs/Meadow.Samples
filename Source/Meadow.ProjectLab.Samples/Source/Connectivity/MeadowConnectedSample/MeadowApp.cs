using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Web.Maple;
using Meadow.Hardware;
using MeadowConnectedSample.Connectivity;
using MeadowConnectedSample.Controller;
using MeadowConnectedSample.Controllers;
using System;
using System.Threading.Tasks;

namespace MeadowConnectedSample
{
    // Change F7CoreComputeV2 to F7FeatherV2 for ProjectLab v2
    public class MeadowApp : App<F7CoreComputeV2>
    {
        bool useWifi = true;

        IProjectLabHardware projectLab;

        public override async Task Initialize()
        {
            Resolver.Log.Info("Initialize...");

            projectLab = ProjectLab.Create();
            Resolver.Log.Info($"Running on ProjectLab Hardware {projectLab.RevisionString}");

            LedController.Instance.Initialize(projectLab.RgbLed);
            LedController.Instance.SetColor(Color.Red);

            MainController.Instance.Initialize(projectLab);
            MainController.Instance.UseWiFi = useWifi;

            DisplayController.Instance.Initialize(projectLab.Display);
            DisplayController.Instance.ShowSplashScreen();

            _ = DisplayController.Instance.StartConnectingAnimation(useWifi);

            if (useWifi)
            {
                var wifi = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();
                wifi.NetworkConnected += WifiNetworkConnected;
                await wifi.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD);
            }
            else
            {
                BluetoothServer.Instance.Initialize();
                LedController.Instance.SetColor(Color.Green);
                _ = MainController.Instance.StartUpdating(TimeSpan.FromSeconds(15));
            }
        }

        private void WifiNetworkConnected(INetworkAdapter sender, NetworkConnectionEventArgs args)
        {
            DisplayController.Instance.StopConnectingAnimation();

            MainController.Instance.StartUpdating(TimeSpan.FromSeconds(15));

            var mapleServer = new MapleServer(sender.IpAddress, 5417, advertise: true, logger: Resolver.Log);
            mapleServer.Start();

            DisplayController.Instance.ShowMapleReady(sender.IpAddress.ToString());

            LedController.Instance.SetColor(Color.Green);
        }
    }
}