using Meadow.Devices;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors.Buttons;
using WifiWeather.Core.Contracts;

namespace WifiWeather.F7.Hardware;

public class WifiWeatherProjectLabHardware : IWifiWeatherHardware
{
    private readonly IProjectLabHardware projLab;
    private readonly INetworkAdapter networkAdapter;

    public RotationType DisplayRotation => RotationType._270Degrees;

    public IButton? UpButton => projLab.UpButton;

    public IButton? DownButton => projLab.DownButton;

    public IPixelDisplay? Display => projLab.Display;

    public INetworkAdapter? NetworkAdapter => networkAdapter;

    public WifiWeatherProjectLabHardware(IProjectLabHardware projLab)
    {
        this.projLab = projLab;

        networkAdapter = projLab.ComputeModule.NetworkAdapters.Primary<IWiFiNetworkAdapter>();
    }
}