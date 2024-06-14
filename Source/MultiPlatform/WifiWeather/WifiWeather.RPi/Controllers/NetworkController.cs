using Meadow;
using System;
using WifiWeather.Core.Contracts;

namespace WifiWeather.RPi.Controllers;

internal class NetworkController : INetworkController
{
    private bool isConnected;

    public event EventHandler? NetworkStatusChanged;

    public NetworkController()
    {
        Resolver.Log.Info($"{MeadowApp.Device.NetworkAdapters.Count} network adapters detected");

        foreach (var adapter in MeadowApp.Device.NetworkAdapters)
        {
            IsConnected = adapter.IsConnected;

            Resolver.Log.Info($"{adapter.Name}  {adapter.IpAddress}");
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
}
