using Meadow;
using Meadow.Peripherals.Displays;
using WifiWeather.Core;
using WifiWeather.Core.Contracts;

namespace WifiWeather.DesktopApp.Hardware
{
    internal class WifiWeatherHardware : IWifiWeatherHardware
    {
        private readonly Desktop device;

        public RotationType DisplayRotation => RotationType.Default;

        public INetworkController? NetworkController { get; }

        public IPixelDisplay? Display => device.Display;

        public WifiWeatherHardware(Desktop device)
        {
            this.device = device;

            NetworkController = new NetworkController();
        }
    }
}