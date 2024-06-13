using Meadow.Peripherals.Displays;

namespace WifiWeather.Core.Contracts;

public interface IWifiWeatherHardware
{
    IPixelDisplay? Display { get; }

    INetworkController NetworkController { get; }
}