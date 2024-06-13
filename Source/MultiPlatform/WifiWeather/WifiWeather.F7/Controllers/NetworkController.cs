using Meadow.Devices;
using Meadow.Hardware;
using System;
using System.Threading.Tasks;
using wifiweather.Core;

namespace wifiweather.F7
{
    internal class NetworkController : INetworkController
    {
        private const string WIFI_NAME = "[SOME_NAME]";
        private const string WIFI_PASSWORD = "[SOME_SECRET]";

        public event EventHandler? NetworkStatusChanged;

        public NetworkController(F7MicroBase device)
        {
            wifi = device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();

            wifi.NetworkConnected += OnNetworkConnected;
            wifi.NetworkDisconnected += OnNetworkDisconnected;
        }

        private void OnNetworkDisconnected(INetworkAdapter sender, NetworkDisconnectionEventArgs args)
        {
            // Handle logic when disconnected.
        }

        private void OnNetworkConnected(INetworkAdapter sender, NetworkConnectionEventArgs args)
        {
            // Handle logic when connected.
        }

        private IWiFiNetworkAdapter? wifi;

        public bool IsConnected
        {
            get => wifi.IsConnected;
        }

        public async Task Connect()
        {
            await wifi.Connect(WIFI_NAME, WIFI_PASSWORD, TimeSpan.FromSeconds(45));
        }
    }
}