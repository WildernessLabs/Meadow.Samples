using Meadow.Devices;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors.Buttons;
using WifiWeather.Core;
using WifiWeather.Core.Contracts;

namespace WifiWeather.F7;

internal class wifiweatherProjectLabHardware : IWifiWeatherHardware
{
    private readonly IProjectLabHardware projLab;

    public RotationType DisplayRotation => RotationType._270Degrees;

    public IButton? LeftButton => projLab.LeftButton;

    public IButton? RightButton => projLab.RightButton;

    public IPixelDisplay? Display => projLab.Display;

    public INetworkController NetworkController { get; }

    public wifiweatherProjectLabHardware(F7CoreComputeV2 device)
    {
        projLab = ProjectLab.Create();

        NetworkController = new NetworkController(device);
    }
}