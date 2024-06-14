using System;

namespace WifiWeather.Core.Contracts;

public interface INetworkController
{
    event EventHandler NetworkStatusChanged;

    bool IsConnected { get; }
}