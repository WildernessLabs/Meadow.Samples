using System;
using System.Threading.Tasks;
using WifiWeather.Core;

namespace WifiWeather.DesktopApp
{
    internal class NetworkController : INetworkController
    {
        private bool isConnected;

        public event EventHandler? NetworkStatusChanged;

        public NetworkController() { }

        public bool IsConnected
        {
            get => isConnected;
            private set
            {
                if (value == IsConnected) { return; }
                isConnected = value;
                NetworkStatusChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public async Task Connect()
        {
            // simulate connection delay
            await Task.Delay(1000);

            IsConnected = true;
        }
    }
}