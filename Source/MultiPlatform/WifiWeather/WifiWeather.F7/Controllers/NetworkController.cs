using Meadow.Devices;
using Meadow.Hardware;
using System;
using WifiWeather.Core.Contracts;

namespace WifiWeather.F7.Controllers;

internal class NetworkController : INetworkController
{
    public event EventHandler? NetworkStatusChanged;

    private IWiFiNetworkAdapter? wifi;

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

    public bool IsConnected
    {
        get => wifi.IsConnected;
    }
}