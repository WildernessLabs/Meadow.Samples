﻿using Meadow.Gateway.WiFi;
using Meadow.Hardware;
using System.Diagnostics;

namespace WiFinder.Core;

public class NetworkController : INetworkController
{
    public event EventHandler<WifiNetwork?>? SelectedNetworkChanged;
    public event EventHandler<List<WifiNetwork>>? NetworkListChanged;
    public event EventHandler<AntennaType>? AntennaChanged;

    private IWiFiNetworkAdapter wifiAdapter;
    private Timer refreshTimer;
    private WifiNetwork? selectedNetwork;
    private AntennaType antennaType;

    public List<WifiNetwork> Networks { get; } = new();
    public WifiNetwork? SelectedNetwork
    {
        get => selectedNetwork;
        private set
        {
            selectedNetwork = value;
            SelectedNetworkChanged?.Invoke(this, SelectedNetwork);
        }
    }

    public NetworkController(IWiFiNetworkAdapter adapter)
    {
        wifiAdapter = adapter;

        refreshTimer = new Timer(RefreshNetworksProc, null, 1000, -1);
    }

    public AntennaType AntennaType
    {
        get => antennaType;
        private set
        {
            if (value == AntennaType) return;
            antennaType = value;
            AntennaChanged?.Invoke(this, AntennaType);
        }
    }

    public virtual void ToggleAntenna()
    {
        AntennaType = AntennaType switch
        {
            AntennaType.OnBoard => AntennaType.External,
            _ => AntennaType.OnBoard
        };
    }

    private async void RefreshNetworksProc(object? _)
    {
        Networks.Clear();

        Networks.AddRange((await wifiAdapter.Scan()).Where(a => a.Ssid.Length > 0));

        foreach (var n in Networks)
        {
            Debug.WriteLine($"{n.SignalDbStrength} {n.Ssid}");
        }

        if (SelectedNetwork == null && Networks.Count > 0)
        {
            // this will just select the first
            SelectPreviousNetwork();
        }

        NetworkListChanged?.Invoke(this, Networks);

        refreshTimer.Change(1000, -1);
    }

    public void SelectPreviousNetwork()
    {
        if (SelectedNetwork == null)
        {
            SelectedNetwork = Networks.FirstOrDefault();
        }
        else
        {
            var previousIndex = Networks.IndexOf(SelectedNetwork) - 1;
            if (previousIndex >= 0)
            {
                SelectedNetwork = Networks[previousIndex];
            }
            else
            {
                SelectedNetwork = Networks.FirstOrDefault();
            }
        }
    }

    public void SelectNextNetwork()
    {
        if (SelectedNetwork == null)
        {
            SelectedNetwork = Networks.FirstOrDefault();
        }
        else
        {
            var nextIndex = Networks.IndexOf(SelectedNetwork) + 1;
            if (nextIndex < Networks.Count)
            {
                SelectedNetwork = Networks[nextIndex];
            }
        }
    }
}
