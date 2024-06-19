using Meadow.Devices;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors.Buttons;
using WifiWeather.Core.Contracts;

namespace WifiWeather.F7.Hardware;

internal class WifiWeatherProjectLabHardware : IWifiWeatherHardware
{
    private readonly IProjectLabHardware projLab;
    private readonly INetworkAdapter networkAdapter;

    public RotationType DisplayRotation => RotationType._270Degrees;

    public IButton? UpButton => projLab.UpButton;

    public IButton? DownButton => projLab.DownButton;

    public IPixelDisplay? Display => projLab.Display;

    public INetworkAdapter NetworkAdapter => networkAdapter;

    public WifiWeatherProjectLabHardware(F7CoreComputeV2 device)
    {
        projLab = ProjectLab.Create();

        networkAdapter = device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();
    }
}