using Meadow;
using System;
using System.Threading.Tasks;
using WifiWeather.Core;

namespace WifiWeather.DesktopApp
{
    internal class NetworkController : INetworkController
    {
        private bool isConnected;

        public event EventHandler? NetworkStatusChanged;

        public NetworkController()
        {
            Resolver.Log.Info($"Meadow.Windows Network Sample");

            Resolver.Log.Info($"{MeadowApp.Device.NetworkAdapters.Count} network adapters detected");
            Resolver.Log.Info($"----------------------------");

            foreach (var adapter in MeadowApp.Device.NetworkAdapters)
            {
                Resolver.Log.Info($"  {adapter.Name}  {adapter.IpAddress}");
            }

            Resolver.Log.Info($"WiFi info");
            Resolver.Log.Info($"----------------------------");
            foreach (var wifi in MeadowApp.Device.NetworkAdapters)
            {
                IsConnected = wifi.IsConnected;

                Resolver.Log.Info($"  {wifi.Name}  {wifi.IpAddress}");
            }
        }

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