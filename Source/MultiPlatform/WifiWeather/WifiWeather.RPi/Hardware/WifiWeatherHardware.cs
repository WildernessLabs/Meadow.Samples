using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Buttons;
using WifiWeather.Core.Contracts;

namespace WifiWeather.RPi.Hardware;

internal class WifiWeatherHardware : IWifiWeatherHardware
{
    private readonly RaspberryPi device;
    private readonly IPixelDisplay? display = null;
    private readonly ITemperatureSensor temperatureSimulator;

    public RotationType DisplayRotation => RotationType.Default;

    public IPixelDisplay? Display => display;

    public IButton? RightButton => null;

    public IButton? LeftButton => null;

    public INetworkController NetworkController { get; }


    public WifiWeatherHardware(RaspberryPi device, bool supportDisplay)
    {
        this.device = device;

        if (supportDisplay)
        {
            // only if we have a display attached
            display = new GtkDisplay(ColorMode.Format16bppRgb565);
        }
    }
}