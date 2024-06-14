using Meadow.Peripherals.Displays;

namespace WifiWeather.Core.Contracts;

public interface IWifiWeatherHardware
{
    RotationType DisplayRotation { get; }

    IPixelDisplay? Display { get; }

    INetworkController NetworkController { get; }
}