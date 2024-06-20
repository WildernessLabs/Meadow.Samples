using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors.Buttons;

namespace WifiWeather.Core.Contracts;

public interface IWifiWeatherHardware
{
    IButton? UpButton { get; }

    IButton? DownButton { get; }

    IPixelDisplay? Display { get; }

    RotationType DisplayRotation { get; }

    INetworkAdapter? NetworkAdapter { get; }
}