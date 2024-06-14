using Meadow.Devices;
using Meadow.Hardware;
using System;
using System.Threading.Tasks;
using WifiWeather.Core;

namespace WifiWeather.F7;

internal class NetworkController : INetworkController
{
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

    }
}